using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Api.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string EntitledPartyId { get; set; }
        public OrderStatus Status { get; set; }
        public int Packages { get; set; }
        public int? TruckbayPickup { get; set; }
    }
}
