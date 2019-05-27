using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSHARE.Models;
using iSHARE.ServiceProvider.Core.Api;
using iSHARE.ServiceProvider.Core.Models;
using iSHARE.ServiceProvider.Core.Requests;

namespace iSHARE.ServiceProvider.Core
{
    public class ContainersService : IContainersService
    {
        private readonly IContainerRepository _repository;
        public ContainersService(IContainerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<Container>> Get(string containerId)
        {

            var container = await _repository.GetByContainerId(containerId);
            if (container != null)
            {
                return Response<Container>.ForSuccess(container);
            }
            return Response.ForNotFound("The specified container was not found.");
        }

        public async Task<Response<Container>> Get(string containerId, string attribute)
        {
            if (!Container.Constants.Attributes.Contains(attribute))
            {
                return Response.ForError("The specified attribute is not found.");
            }
            var container = await _repository.GetByContainerId(containerId);
            if (container != null)
            {
                return Response<Container>.ForSuccess(container);
            }
            return Response.ForNotFound("The specified container was not found.");
        }


        public async Task<Response<IReadOnlyCollection<Container>>> GetByEntitledParty(string entitledPartyId)
        {
            var containers = await _repository.GetByEntitledPartyId(entitledPartyId);

            return Response<IReadOnlyCollection<Container>>.ForSuccess(containers);
        }

        public async Task<Response<Container>> Create(CreateContainerRequest request)
        {
            var validationResult = await ValidateCreate(request);
            if (!validationResult.Success)
            {
                return Response.ForErrors(validationResult.Errors);
            }

            var container = await _repository.Insert(
                new Container
                {
                    ContainerId = request.ContainerId,
                    EntitledPartyId = request.EntitledPartyId,
                    Weight = request.Weight,
                    Eta = request.Eta
                });
            return Response<Container>.ForSuccess(container);
        }

        public async Task<Response<Container>> Edit(EditContainerRequest request)
        {
            var validationResult = ValidateProperties(request.ContainerData?.Eta, request.ContainerData?.Weight);
            if (!validationResult.Success)
            {
                return Response.ForErrors(validationResult.Errors);
            }

            var container = await _repository.GetByContainerId(request.ContainerId);

            if (container != null)
            {
                var updatedContainer = await _repository.Update(new Container
                {
                    ContainerId = container.ContainerId,
                    EntitledPartyId = container.EntitledPartyId,
                    Weight = request.ContainerData.Weight,
                    Eta = request.ContainerData.Eta
                });
                return Response<Container>.ForSuccess(updatedContainer);
            }
            return Response.ForNotFound("The specified container was not found.");
        }

        private async Task<Response> ValidateCreate(CreateContainerRequest request)
        {
            var container = await _repository.GetByContainerId(request.ContainerId);
            if (container != null)
            {
                return Response.ForError("The container already exists.");
            }

            return ValidateProperties(request.Eta, request.Weight);
        }

        private Response ValidateProperties(string eta, decimal? weight)
        {
            if (!int.TryParse(eta, out int result) || result < 0)
            {
                return Response.ForError("The eta field is invalid.");
            }
            if (decimal.Round(weight.Value, 2) != weight || weight < 0)
            {
                return Response.ForError("The weight field is invalid.");
            }

            return Response.ForSuccess();
        }

        public async Task<Response<Container>> Delete(string containerId)
        {
            if (await _repository.GetByContainerId(containerId) != null)
            {
                var container = await _repository.Delete(containerId);
                return Response<Container>.ForSuccess(container);
            }

            return Response.ForNotFound("The specified container was not found.");
        }

        public async Task<Response<Container>> Update(PatchContainerRequest request)
        {
            var container = await Get(request.ContainerId);
            if (container.Model == null)
            {
                return Response.ForNotFound("The specified container was not found.");
            }

            var patch = new ContainerPatch
            {
                Eta = container.Model.Eta,
                Weight = container.Model.Weight
            };

            request.PatchData.ApplyTo(patch);

            container.Model.Weight = patch.Weight;
            container.Model.Eta = patch.Eta;

            var validationResult = ValidateProperties(container.Model.Eta, container.Model.Weight);
            if (!validationResult.Success)
            {
                return Response.ForErrors(validationResult.Errors);
            }
            var patchedContainer = await _repository.Update(container.Model);

            return Response<Container>.ForSuccess(patchedContainer);
        }

    }
}
