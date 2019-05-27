using Newtonsoft.Json;

namespace iSHARE.ServiceProvider.Api.ViewModels
{
    public class ContainerViewModel
    {
        [JsonProperty("container_id")]
        public string ContainerId { get; set; }
        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Weight { get; set; }
        [JsonProperty("eta", NullValueHandling = NullValueHandling.Ignore)]
        public string Eta { get; set; }

    }
}
