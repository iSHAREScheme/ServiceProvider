using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.Models;
using iSHARE.ServiceProvider.Core.Models;
using iSHARE.ServiceProvider.Core.Requests;

namespace iSHARE.ServiceProvider.Core.Api
{
    public interface IContainersService
    {
        Task<Response<Container>> Get(string containerId);
        Task<Response<Container>> Get(string containerId, string attribute);
        Task<Response<IReadOnlyCollection<Container>>> GetByEntitledParty(string entitledPartyId);
        Task<Response<Container>> Create(CreateContainerRequest request);
        Task<Response<Container>> Edit(EditContainerRequest request);
        Task<Response<Container>> Delete(string containerId);
        Task<Response<Container>> Update(PatchContainerRequest request);
    }
}
