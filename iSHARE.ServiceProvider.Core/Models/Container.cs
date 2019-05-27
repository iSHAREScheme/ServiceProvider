using System;
using System.Collections.Generic;

namespace iSHARE.ServiceProvider.Core.Models
{
    public class Container
    {
        public Guid Id { get; set; }
        public string EntitledPartyId { get; set; }
        public decimal? Weight { get; set; }
        public string Eta { get; set; }
        public string ContainerId { get; set; }

        public static class Constants
        {
            public static readonly string All = "*";
            public static readonly string ResourceType = "GS1.CONTAINER";
            public static readonly string Eta = "GS1.CONTAINER.ATTRIBUTE.ETA";
            public static readonly string Weight = "GS1.CONTAINER.ATTRIBUTE.WEIGHT";
            public static readonly IReadOnlyCollection<string> Attributes = new[] { Eta, Weight };
        }
    }
}
