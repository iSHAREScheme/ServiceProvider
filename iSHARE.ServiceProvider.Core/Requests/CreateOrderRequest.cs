using System.ComponentModel.DataAnnotations;
using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Core.Requests
{
    public class CreateOrderRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string OrderId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string EntitledPartyId { get; set; }
        [Required]
        public OrderStatus? Status { get; set; }
        [Required]
        public int? Packages { get; set; }
        public int? TruckbayPickup { get; set; }
    }
}
