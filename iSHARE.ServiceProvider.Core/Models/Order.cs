using System;

namespace iSHARE.ServiceProvider.Core.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string EntitledPartyId { get; set; }
        public OrderStatus Status { get; set; }
        public int Packages { get; set; }
        public int? TruckbayPickup { get; set; }
    }
}
