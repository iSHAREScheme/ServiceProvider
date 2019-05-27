using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Core
{
    public interface IOrderRepository
    {
        Task<Order> GetByOrderId(string orderId);
        Task<IReadOnlyCollection<Order>> GetByEntitledPartyId(string entitledPartyId);
        Task<Order> Insert(Order order);
        Task<Order> Update(Order order);
        Task<Order> Delete(string orderId);
    }
}
