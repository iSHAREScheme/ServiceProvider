using System.Threading.Tasks;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public class DenyAuthorizationService : IResourceAuthorizationService
    {
        public Task<bool> Allow(ResourceAuthorizationRequest resourceAuthorizationRequest) => Task.FromResult(false);
    }
}