using System.Collections.Generic;
using iSHARE.Models.DelegationMask;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public class ResourceAuthorizationRequest
    {
        public string AccessSubject { get; set; }
        public string EntitledPartyId { get; internal set; }
        public string ServiceProviderId { get; internal set; }
        public IReadOnlyCollection<string> Identifiers { get; internal set; }
        public IReadOnlyCollection<string> Attributes { get; internal set; }
        public IReadOnlyCollection<string> Actions { get; internal set; }
        public string ResourceType { get; set; }
        public DelegationRequestPolicySet CustomPolicies { get; set; }
    }
}
