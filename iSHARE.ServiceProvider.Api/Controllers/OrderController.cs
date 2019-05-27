using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSHARE.Api.Controllers;
using iSHARE.ServiceProvider.Api.ViewModels;
using iSHARE.ServiceProvider.Core.Api;
using iSHARE.ServiceProvider.Core.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace iSHARE.ServiceProvider.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderController : SchemeAuthorizedController
    {
        private readonly IOrdersService _ordersService;
        public OrderController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        /// <summary>
        /// Example service of the Service Provider
        /// </summary>
        /// <remarks>
        /// This is an example service to show how any Service Provider that adheres to iSHARE MUST apply iSHARE I OAuth to every iSHARE enabled service. 
        /// </remarks>
        /// <param name="orderName">The identifier of the order requested</param>
        /// <param name="attribute">The type of the attribute requested</param>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpGet, Route("order")]
        public ActionResult<OrderViewModel> Get(
            [FromQuery, SwaggerParameter(Required = true)]string orderName,
            [FromQuery, SwaggerParameter(Required = true)]string attribute)
        {
            return Ok(new OrderViewModel { OrderId = orderName });
        }

        /// <summary>
        /// Example service of the Service Provider
        /// </summary>
        /// <remarks>
        /// This is an example service to show how any Service Provider that adheres to iSHARE MUST apply iSHARE I OAuth to every iSHARE enabled service. 
        /// </remarks>
        /// <param name="entitledPartyId">The identifier of the entitled party for which all orders are requested</param>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpGet("parties/{entitledPartyId}/orders")]
        public async Task<ActionResult<IList<OrderViewModel>>> Get(string entitledPartyId)
        {
            var models = await _ordersService.GetByEntitledParty(entitledPartyId);
            return Ok(models.Map().ToList());
        }

        [HttpPost("order")]
        public async Task<ActionResult<OrderViewModel>> Create([FromBody]CreateOrderRequest request)
        {
            var response = await _ordersService.Create(request);
            return FromResponse(response, OrdersMappings.Map);
        }

        [HttpPut("order")]
        public async Task<ActionResult<OrderViewModel>> Edit([FromBody]EditOrderRequest request)
        {
            var response = await _ordersService.Edit(request);
            return FromResponse(response, OrdersMappings.Map);
        }

        [HttpDelete("order/{orderId}")]
        public async Task<ActionResult<OrderViewModel>> Edit(string orderId)
        {
            var response = await _ordersService.Delete(orderId);
            return FromResponse(response, OrdersMappings.Map);
        }
    }
}
