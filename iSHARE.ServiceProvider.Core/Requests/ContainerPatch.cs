using Newtonsoft.Json;

namespace iSHARE.ServiceProvider.Core.Requests
{
    public class ContainerPatch
    {
        [JsonProperty("weight")]
        public decimal? Weight { get; set; }
        [JsonProperty("eta")]
        public string Eta { get; set; }
    }
}
