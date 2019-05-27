using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace iSHARE.ServiceProvider.Core.Requests
{
    public class ContainerEdit
    {
        [Required]
        [JsonProperty("weight")]
        public decimal? Weight { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("eta")]
        public string Eta { get; set; }
    }
}
