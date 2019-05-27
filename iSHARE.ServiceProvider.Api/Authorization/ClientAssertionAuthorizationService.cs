using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using iSHARE.Configuration.Configurations;
using iSHARE.IdentityServer.Validation.Interfaces;
using iSHARE.Models.DelegationMask;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public class ClientAssertionAuthorizationService : IResourceAuthorizationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISecretValidator _secretValidator;
        private readonly IAuthorizationRegistryClient _client;
        private readonly ILogger _logger;
        private readonly PartyDetailsOptions _options;
        public ClientAssertionAuthorizationService(
            IHttpContextAccessor contextAccessor,
            ISecretValidator secretValidator,
            IAuthorizationRegistryClient client,
            ILogger<ClientAssertionAuthorizationService> logger,
            PartyDetailsOptions options)
        {
            _contextAccessor = contextAccessor;
            _secretValidator = secretValidator;
            _client = client;
            _logger = logger;
            _options = options;
        }
        public async Task<bool> Allow(ResourceAuthorizationRequest resourceAuthorizationRequest)
        {
            string clientAssertionToken = _contextAccessor.HttpContext.Request.Headers["client_assertion"];

            if (string.IsNullOrWhiteSpace(clientAssertionToken))
            {
                return await Task.FromResult(false);
            }

            try
            {
                var result = await _secretValidator.ValidateAsync(new List<Secret>(), new ParsedSecret()
                {
                    Id = resourceAuthorizationRequest.AccessSubject,
                    Type = OidcConstants.GrantTypes.JwtBearer,
                    Credential = clientAssertionToken
                });
                if (result.Success)
                {
                    DelegationMask mask = null;
                    if (resourceAuthorizationRequest.CustomPolicies == null)
                    {
                        mask = new DelegationMask(resourceAuthorizationRequest.Identifiers,
                        resourceAuthorizationRequest.EntitledPartyId,
                        resourceAuthorizationRequest.AccessSubject,
                        resourceAuthorizationRequest.Attributes,
                        resourceAuthorizationRequest.Actions,
                        resourceAuthorizationRequest.ResourceType,
                        _options.ClientId);

                        var delegationEvidence = await _client.GetDelegation(mask, clientAssertionToken);

                        if (delegationEvidence == null)
                        {
                            _logger.LogWarning("No response from AR");
                            return await Task.FromResult(false);
                        }

                        if (!delegationEvidence.Permit(resourceAuthorizationRequest.Identifiers,
                            resourceAuthorizationRequest.Attributes,
                            resourceAuthorizationRequest.Actions,
                            resourceAuthorizationRequest.ServiceProviderId))
                        {
                            _logger.LogWarning($"Access denied to {resourceAuthorizationRequest.Identifiers}");

                            return await Task.FromResult(false);
                        }
                    }
                    else
                    {
                        mask = new DelegationMask(resourceAuthorizationRequest.EntitledPartyId,
                            resourceAuthorizationRequest.AccessSubject,
                        resourceAuthorizationRequest.CustomPolicies);

                        var delegationEvidence = await _client.GetDelegation(mask, clientAssertionToken);

                        if (delegationEvidence == null)
                        {
                            _logger.LogWarning("No response from AR");
                            return await Task.FromResult(false);
                        }

                        if (!delegationEvidence.Permit(resourceAuthorizationRequest.ServiceProviderId, resourceAuthorizationRequest.CustomPolicies))
                        {
                            _logger.LogWarning($"Access denied to {resourceAuthorizationRequest.Identifiers}");

                            return await Task.FromResult(false);
                        }
                    }

                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Access denied to {resourceAuthorizationRequest.Identifiers}", ex);
                return await Task.FromResult(false);
            }
        }
    }
}
