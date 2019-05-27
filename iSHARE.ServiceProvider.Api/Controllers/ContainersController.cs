using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.Api.Controllers;
using iSHARE.Api.Swagger.Authorization;
using iSHARE.ServiceProvider.Api.ViewModels;
using iSHARE.ServiceProvider.Core.Api;
using iSHARE.ServiceProvider.Core.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace iSHARE.ServiceProvider.Api.Controllers
{
    [ServiceConsumer]
    public class ContainersController : SchemeAuthorizedController
    {
        private readonly IContainersService _containersService;

        public ContainersController(IContainersService containersService)
        {
            _containersService = containersService;
        }

        [HttpGet("containers/{containerId}")]
        public async Task<ActionResult<ContainerViewModel>> GetById(
            [SwaggerParameter(Required = true, Description = "The identifier of the requested container")]string containerId)
        {
            var response = await _containersService.Get(containerId);

            return FromResponse(response, ContainersMappings.Map);
        }

        [HttpGet("containers/{containerId}/{attribute}")]
        public async Task<ActionResult<ContainerViewModel>> GetByAttributeId(
            [SwaggerParameter(Required = true, Description = "The identifier of the requested container")]string containerId,
            [SwaggerParameter(Required = true, Description = "The attribute requested for the specified container")] string attribute)
        {
            var response = await _containersService.Get(containerId, attribute);
            return FromResponse(response, c => ContainersMappings.Map(c, attribute));
        }

        [HttpGet("parties/{entitledPartyId}/containers")]
        public async Task<ActionResult<IList<ContainerViewModel>>> Get(
            [SwaggerParameter(Required = true, Description = "The identifier of the entitled party for which all containers are requested")]string entitledPartyId)
        {
            var response = await _containersService.GetByEntitledParty(entitledPartyId);
            return FromResponse<IReadOnlyCollection<Core.Models.Container>>(response, ContainersMappings.Map);
        }

        [HttpPost("containers")]
        public async Task<ActionResult<ContainerViewModel>> Create(
            [FromBody, SwaggerParameter(Required = true, Description = "The create request")]CreateContainerRequest request)
        {
            var response = await _containersService.Create(request);

            return FromResponse(response, ContainersMappings.Map);
        }

        [HttpPut("containers/{containerId}")]
        public async Task<ActionResult<ContainerViewModel>> Edit(
            [SwaggerParameter(Required = true, Description = "The identifier of the container for which the update operation will proceed")]string containerId,
            [FromBody, SwaggerParameter(Required = true, Description = "The update request")]ContainerEdit request)
        {
            var response = await _containersService.Edit(new EditContainerRequest { ContainerId = containerId, ContainerData = request });
            return FromResponse(response, ContainersMappings.Map);
        }

        [HttpDelete("containers/{containerId}")]
        public async Task<ActionResult<ContainerViewModel>> Delete(
            [SwaggerParameter(Required = true, Description = "The identifier of the container for which the delete operation will proceed")]string containerId)
        {
            var response = await _containersService.Delete(containerId);

            return FromResponse(response, ContainersMappings.Map);
        }

        [HttpPatch("containers/{containerId}")]
        public async Task<ActionResult<ContainerViewModel>> Patch(
            [SwaggerParameter(Required = true, Description = "The identifier of the container for which the patch operation will proceed")]string containerId,
            [FromBody, SwaggerParameter(Required = true, Description = "The patch request")]JsonPatchDocument<ContainerPatch> patchData)
        {
            var response = await _containersService.Update(
                new PatchContainerRequest
                {
                    ContainerId = containerId,
                    PatchData = patchData
                });
            return FromResponse(response, ContainersMappings.Map);
        }
    }
}
