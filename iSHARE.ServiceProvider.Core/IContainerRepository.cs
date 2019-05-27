using System.Collections.Generic;
using System.Threading.Tasks;
using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Core
{
    public interface IContainerRepository
    {
        Task<Container> GetByContainerId(string containerId);
        Task<IReadOnlyCollection<Container>> GetByEntitledPartyId(string entitledPartyId);
        Task<Container> Insert(Container container);
        Task<Container> Update(Container container);
        Task<Container> Delete(string containerId);
    }
}