using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.Models;
using iSHARE.ServiceProvider.Core.Models;
using iSHARE.ServiceProvider.Core.Requests;

namespace iSHARE.ServiceProvider.Core.Api
{
    public interface IOrdersService
    {
        Task<Order> GetAsync(string orderId);
        Task<IReadOnlyCollection<Order>> GetByEntitledParty(string entitledPartyId);
        Task<Response<Order>> Create(CreateOrderRequest request);
        Task<Response<Order>> Edit(EditOrderRequest request);
        Task<Response<Order>> Delete(string orderId);
    }
}
