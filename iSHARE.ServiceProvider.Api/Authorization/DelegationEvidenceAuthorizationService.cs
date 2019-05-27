using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using iSHARE.AuthorizationRegistry.Client;
using iSHARE.Models.DelegationEvidence;
using Manatee.Json;
using Manatee.Json.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public class DelegationEvidenceAuthorizationService : IResourceAuthorizationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;
        private readonly IJsonSchema _jsonSchema;
        private readonly AuthorizationRegistryClientOptions _arOptions;

        public DelegationEvidenceAuthorizationService(IHttpContextAccessor contextAccessor,
            ILogger<DelegationEvidenceAuthorizationService> logger,
            Func<string, IJsonSchema> jsonSchemaFactory,
            AuthorizationRegistryClientOptions arOptions)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _jsonSchema = jsonSchemaFactory("delegationEvidenceSchema.json");
            _arOptions = arOptions;
        }

        public Task<bool> Allow(ResourceAuthorizationRequest resourceAuthorizationRequest)
        {
            var delegationEvidenceToken = _contextAccessor.HttpContext.Request.Headers["delegation_evidence"];
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(delegationEvidenceToken);

                var x5CCerts = jwtToken.Header["x5c"].ToString();
                if (string.IsNullOrEmpty(x5CCerts))
                {
                    _logger.LogWarning("No x5c header provided.");
                    return Task.FromResult(false);
                }

                var chain = JsonConvert.DeserializeObject<string[]>(x5CCerts);

                var validationParams = GetValidationParameters(chain[0],
                       new[] { resourceAuthorizationRequest.AccessSubject },
                       _arOptions.ClientId);

                var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken.RawData, validationParams,
                    out SecurityToken securityToken);

                var delegationEvidence = jwtToken.Claims.FirstOrDefault(c => c.Type == "delegationEvidence").Value?.ToString();

                if (string.IsNullOrEmpty(delegationEvidence))
                {
                    _logger.LogWarning("Delegation evidence not provided in token.");
                    return Task.FromResult(false);
                }

                var result = _jsonSchema.Validate(JsonValue.Parse(delegationEvidence));

                if (!result.Valid)
                {
                    var errors = result.Errors.Select(e => e.Message + " " + e.PropertyName).ToList();
                    var errorMessage = errors.Aggregate("", (s, i) => "" + s + "," + i);
                    _logger.LogWarning($"Errors during policy mask validation: {errorMessage}");
                    return Task.FromResult(false);
                }

                var policy = GetDelegationEvidence(jwtToken);

                if (policy.PolicyIssuer != resourceAuthorizationRequest.EntitledPartyId)
                {
                    return Task.FromResult(false);
                }

                bool validationResult = false;
                if (resourceAuthorizationRequest.CustomPolicies == null)
                {
                    validationResult = policy.Permit(resourceAuthorizationRequest.Identifiers,
                        resourceAuthorizationRequest.Attributes,
                        resourceAuthorizationRequest.Actions,
                        resourceAuthorizationRequest.ServiceProviderId);
                }
                else
                {
                    validationResult = policy.Permit(resourceAuthorizationRequest.ServiceProviderId, resourceAuthorizationRequest.CustomPolicies);
                }

                if (!validationResult)
                {
                    _logger.LogWarning("Errors during policy mask validation");
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Access denied to {resourceAuthorizationRequest.Identifiers}", ex);
                return Task.FromResult(false);
            }
        }

        private TokenValidationParameters GetValidationParameters(string publicKey, string[] audiences, string issuer)
        {
            var cert = new X509Certificate2(Encoding.ASCII.GetBytes(publicKey));
            var key = new X509SecurityKey(cert);
            return new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidAudiences = audiences,
                ValidIssuer = issuer
            };
        }

        private static DelegationEvidence GetDelegationEvidence(JwtSecurityToken token)
            => JsonConvert.DeserializeObject<DelegationEvidence>(token.Claims.First(c => c.Type == "delegationEvidence").Value);

    }
}
