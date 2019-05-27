using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSHARE.Configuration;
using iSHARE.Configuration.Configurations;
using iSHARE.Models;
using iSHARE.Models.DelegationEvidence;
using iSHARE.Models.DelegationMask;
using iSHARE.ServiceProvider.Core.Api;
using iSHARE.ServiceProvider.Core.Models;
using iSHARE.ServiceProvider.Core.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public class AuthorizationContainersServiceDecorator : IContainersService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IContainersService _containersService;
        private readonly IResourceAuthorizationService _authorizationService;
        private readonly PartyDetailsOptions _partyDetailsOptions;

        public AuthorizationContainersServiceDecorator(Decorator<IContainersService> containersServiceImplementation,
            IHttpContextAccessor context,
            IResourceAuthorizationService authorizationService,
            PartyDetailsOptions partyDetailsOptions
            )
        {
            _context = context;
            _authorizationService = authorizationService;
            _partyDetailsOptions = partyDetailsOptions;
            _containersService = containersServiceImplementation.Instance;
        }

        public async Task<Response<Container>> Get(string containerId)
        {
            var container = await _containersService.Get(containerId);
            if (!container.Success)
            {
                return container;
            }

            var authorization = InitFor(container.Model);
            authorization.Attributes = Container.Constants.Attributes;
            authorization.Actions = new[] { Actions.Read };


            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }

            return container;
        }
        public async Task<Response<Container>> Get(string containerId, string attribute)
        {
            var container = await _containersService.Get(containerId, attribute);
            if (!container.Success)
            {
                return container;
            }

            var authorization = InitFor(container.Model);
            authorization.Attributes = new[] { attribute };
            authorization.Actions = new[] { Actions.Read };

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }

            return container;
        }

        public async Task<Response<IReadOnlyCollection<Container>>> GetByEntitledParty(string entitledPartyId)
        {
            var authorization = InitFor();
            authorization.EntitledPartyId = entitledPartyId;
            authorization.Identifiers = new[] { Container.Constants.All };
            authorization.Attributes = Container.Constants.Attributes;
            authorization.Actions = new[] { Actions.Read };

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }

            return await _containersService.GetByEntitledParty(entitledPartyId);
        }

        public async Task<Response<Container>> Create(CreateContainerRequest request)
        {
            var authorization = InitFor();
            authorization.EntitledPartyId = request.EntitledPartyId;
            authorization.Identifiers = new[] { request.ContainerId };
            authorization.Attributes = Container.Constants.Attributes;
            authorization.Actions = new[] { Actions.Create };

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }
            return await _containersService.Create(request);
        }



        public async Task<Response<Container>> Edit(EditContainerRequest request)
        {
            var container = await _containersService.Get(request.ContainerId);
            if (!container.Success)
            {
                return Response.ForNotFound();
            }

            var authorization = InitFor(container.Model);
            authorization.Attributes = new[] { Container.Constants.Eta, Container.Constants.Weight };
            authorization.Actions = new[] { Actions.Update };

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }

            return await _containersService.Edit(request);
        }

        public async Task<Response<Container>> Delete(string containerId)
        {
            var container = await _containersService.Get(containerId);
            if (!container.Success)
            {
                return container;
            }

            var authorization = InitFor(container.Model);
            authorization.Attributes = Container.Constants.Attributes;
            authorization.Actions = new[] { Actions.Delete };

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }
            return await _containersService.Delete(containerId);
        }

        public async Task<Response<Container>> Update(PatchContainerRequest request)
        {
            var container = await _containersService.Get(request.ContainerId);
            if (!container.Success)
            {
                return Response.ForNotFound();
            }

            var authorization = InitFor(container.Model);
            authorization.CustomPolicies = CreatePatchCustomPolicies(request);

            if (!await _authorizationService.Allow(authorization))
            {
                return Response.ForNotAuthorized();
            }

            return await _containersService.Update(request);
        }

        private ResourceAuthorizationRequest InitFor() => new ResourceAuthorizationRequest
        {
            AccessSubject = _context.HttpContext.User.GetRequestingClientId(),
            ServiceProviderId = _partyDetailsOptions.ClientId,
            ResourceType = Container.Constants.ResourceType,
        };

        private ResourceAuthorizationRequest InitFor(Container container)
        {
            var request = InitFor();

            request.EntitledPartyId = container.EntitledPartyId;
            request.Identifiers = new[] { container.ContainerId };

            return request;
        }




        private DelegationRequestPolicySet CreatePatchCustomPolicies(PatchContainerRequest request)
        {
            return new DelegationRequestPolicySet
            {
                Policies = request.PatchData.Operations.Select(x =>
                {
                    string action = null;
                    switch (x.OperationType)
                    {
                        case OperationType.Replace:
                            action = Actions.Update;
                            break;
                        case OperationType.Remove:
                            action = Actions.Delete;
                            break;
                        case OperationType.Add:
                            action = Actions.Create;
                            break;
                        default:
                            throw new InvalidOperationException($"OperationType {x.OperationType} is not supported.");
                    }

                    string attribute;
                    switch (x.path)
                    {
                        case "/weight":
                            attribute = Container.Constants.Weight;
                            break;
                        case "/eta":
                            attribute = Container.Constants.Eta;
                            break;
                        default:
                            throw new InvalidOperationException($"Attribute {x.path} is not recognized.");
                    }

                    var policy = new Policy
                    {
                        Target = new PolicyTarget
                        {
                            Resource = new PolicyTargetResource
                            {
                                Type = Container.Constants.ResourceType,
                                Identifiers = new List<string> { request.ContainerId },
                                Attributes = new List<string> { attribute }
                            },
                            Actions = new List<string> { action },
                            Environment = new PolicyTargetEnvironment
                            {
                                ServiceProviders = new List<string> { _partyDetailsOptions.ClientId }
                            }
                        },
                        Rules = new List<PolicyRule> { PolicyRule.Permit() }
                    };
                    return policy;

                }).ToList()
            };
        }
    }
}
