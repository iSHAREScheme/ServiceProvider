using System.Collections.Generic;
using System.Linq;
using iSHARE.ServiceProvider.Core.Models;

namespace iSHARE.ServiceProvider.Api.ViewModels
{
    internal static class ContainersMappings
    {
        public static IEnumerable<ContainerViewModel> Map(this IEnumerable<Container> models)
            => models.Select(Map);

        public static ContainerViewModel Map(this Container model)
            => model != null ? new ContainerViewModel
            {
                ContainerId = model.ContainerId,
                Weight = model.Weight,
                Eta = model.Eta,
            } : null;

        public static ContainerViewModel MapToEta(this Container model)
            => model != null ? new ContainerViewModel
            {
                ContainerId = model.ContainerId,
                Eta = model.Eta,
            } : null;

        public static ContainerViewModel MapToWeight(this Container model)
            => model != null ? new ContainerViewModel
            {
                ContainerId = model.ContainerId,
                Weight = model.Weight,
            } : null;

        internal static ContainerViewModel Map(Container container, string attribute)
        {
            if (attribute == null)
            {
                return Map(container);
            }

            if (attribute == Container.Constants.Eta)
            {
                return MapToEta(container);
            }

            if (attribute == Container.Constants.Weight)
            {
                return MapToWeight(container);
            }
            return null;
        }
    }
}
