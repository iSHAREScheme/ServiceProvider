using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSHARE.ServiceProvider.Core;
using iSHARE.ServiceProvider.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iSHARE.ServiceProvider.Data
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly ServiceProviderDbContext _db;
        public ContainerRepository(ServiceProviderDbContext db)
        {
            _db = db;
        }

        public async Task<Container> GetByContainerId(string containerId)
        {
            var entity = await _db.Containers.FirstOrDefaultAsync(c => c.ContainerId == containerId);
            return entity;
        }

        public async Task<IReadOnlyCollection<Container>> GetByEntitledPartyId(string entitledPartyId)
        {
            var entities = await _db.Containers.Where(c => c.EntitledPartyId == entitledPartyId).ToListAsync();

            return entities;
        }

        public async Task<Container> Insert(Container container)
        {
            await _db.Containers.AddAsync(container);
            await _db.SaveChangesAsync();
            return container;
        }

        public async Task<Container> Update(Container container)
        {
            var updatedContainer = await _db.Containers.FirstOrDefaultAsync(x => x.ContainerId == container.ContainerId);

            updatedContainer.EntitledPartyId = container.EntitledPartyId;
            updatedContainer.Eta = container.Eta;
            updatedContainer.Weight = container.Weight;
            await _db.SaveChangesAsync();

            return container;
        }

        public async Task<Container> Delete(string containerId)
        {
            var container = await _db.Containers.FirstOrDefaultAsync(x => x.ContainerId == containerId);

            _db.Containers.Remove(container);
            await _db.SaveChangesAsync();

            return container;
        }
    }
}
