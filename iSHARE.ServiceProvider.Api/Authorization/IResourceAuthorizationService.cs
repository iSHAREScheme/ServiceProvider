using System.Threading.Tasks;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public interface IResourceAuthorizationService
    {
        Task<bool> Allow(ResourceAuthorizationRequest resourceAuthorizationRequest);
    }
}