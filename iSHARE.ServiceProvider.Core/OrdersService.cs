using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.Models;
using iSHARE.ServiceProvider.Core.Api;
using iSHARE.ServiceProvider.Core.Models;
using iSHARE.ServiceProvider.Core.Requests;

namespace iSHARE.ServiceProvider.Core
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _repository;

        public OrdersService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public Task<Order> GetAsync(string orderId) => _repository.GetByOrderId(orderId);

        public async Task<IReadOnlyCollection<Order>> GetByEntitledParty(string entitledPartyId)
        {

            var orders = await _repository.GetByEntitledPartyId(entitledPartyId);

            return orders;
        }

        public async Task<Response<Order>> Create(CreateOrderRequest request)
        {
            var validationResult = await ValidateCreate(request);
            if (!validationResult.Success)
            {
                return Response.ForErrors(validationResult.Errors);
            }

            var order = await _repository.Insert(new Order
            {
                OrderId = request.OrderId,
                EntitledPartyId = request.EntitledPartyId,
                Packages = request.Packages.Value,
                Status = request.Status.Value,
                TruckbayPickup = request.TruckbayPickup

            });

            return Response<Order>.ForSuccess(order);
        }

        public async Task<Response<Order>> Edit(EditOrderRequest request)
        {
            var validationResult = ValidateProperties(request.Packages, request.TruckbayPickup);
            if (!validationResult.Success)
            {
                return Response.ForErrors(validationResult.Errors);
            }

            var order = await _repository.GetByOrderId(request.OrderId);

            if (order != null)
            {
                var updatedOrder = await _repository.Update(new Order
                {
                    OrderId = request.OrderId,
                    EntitledPartyId = request.EntitledPartyId,
                    Packages = request.Packages.Value,
                    Status = request.Status.Value,
                    TruckbayPickup = request.TruckbayPickup

                });
                return Response<Order>.ForSuccess(updatedOrder);
            }

            return Response.ForError("The specified order was not found.");
        }

        private async Task<Response> ValidateCreate(CreateOrderRequest request)
        {
            var order = await _repository.GetByOrderId(request.OrderId);
            if (order != null)
            {
                return Response.ForError("The order already exists.");
            }
            return ValidateProperties(request.Packages, request.TruckbayPickup);
        }

        private Response ValidateProperties(int? packages, int? truckbayPickup)
        {
            if (packages.Value < 0)
            {
                return Response.ForError("The packages field is invalid.");
            }

            if (truckbayPickup.HasValue && truckbayPickup.Value < 0)
            {
                return Response.ForError("The truckbayPickup field is invalid.");
            }

            return Response.ForSuccess();
        }

        public async Task<Response<Order>> Delete(string orderId)
        {
            if ((await _repository.GetByOrderId(orderId)) != null)
            {
                var order = await _repository.Delete(orderId);
                return Response<Order>.ForSuccess(order);
            }

            return Response.ForError("The specified order was not found.");
        }
    }
}
