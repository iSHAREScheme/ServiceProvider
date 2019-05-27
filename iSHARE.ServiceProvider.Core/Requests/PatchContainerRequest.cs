using Microsoft.AspNetCore.JsonPatch;

namespace iSHARE.ServiceProvider.Core.Requests
{
    public class PatchContainerRequest
    {
        public string ContainerId { get; set; }
        public JsonPatchDocument<ContainerPatch> PatchData { get; set; }
    }
}
