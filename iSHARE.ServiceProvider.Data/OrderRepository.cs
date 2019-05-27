using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSHARE.ServiceProvider.Core;
using iSHARE.ServiceProvider.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iSHARE.ServiceProvider.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ServiceProviderDbContext _db;
        public OrderRepository(ServiceProviderDbContext db)
        {
            _db = db;
        }
        public async Task<Order> GetByOrderId(string orderId)
        {
            var entity = await _db.Orders.FirstOrDefaultAsync(c => c.OrderId == orderId);
            if (entity != null)
            {
                return new Order
                {
                    OrderId = entity.OrderId,
                    EntitledPartyId = entity.EntitledPartyId
                };
            }

            return null;
        }

        public async Task<IReadOnlyCollection<Order>> GetByEntitledPartyId(string entitledPartyId)
        {
            var entities = await _db.Orders.Where(c => c.EntitledPartyId == entitledPartyId).ToListAsync();

            return entities.ToList();
        }

        public async Task<Order> Insert(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<Order> Update(Order order)
        {
            var updatedOrder = await _db.Orders.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);

            updatedOrder.EntitledPartyId = order.EntitledPartyId;
            updatedOrder.Packages = order.Packages;
            updatedOrder.TruckbayPickup = order.TruckbayPickup;
            updatedOrder.Status = order.Status;
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order> Delete(string orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            return order;
        }
    }
}
