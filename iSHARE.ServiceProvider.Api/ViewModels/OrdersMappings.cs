using System.Collections.Generic;
using System.Linq;
using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Api.ViewModels
{
    public static class OrdersMappings
    {
        public static IEnumerable<OrderViewModel> Map(this IEnumerable<Order> models)
            => models.Select(Map);

        public static OrderViewModel Map(this Order order)
        {
            return new OrderViewModel
            {
                OrderId = order.OrderId,
                EntitledPartyId = order.EntitledPartyId,
                Packages = order.Packages,
                TruckbayPickup = order.TruckbayPickup,
                Status = order.Status
            };
        }


    }
}
