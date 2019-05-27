using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace iSHARE.ServiceProvider.Core.Requests
{
    public class CreateContainerRequest
    {
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("container_id")]
        public string ContainerId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("entitled_party_id")]
        public string EntitledPartyId { get; set; }
        [Required]
        [JsonProperty("weight")]
        public decimal? Weight { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("eta")]
        public string Eta { get; set; }
    }
}
