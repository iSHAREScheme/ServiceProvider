using System;
using System.Collections.Generic;
using System.Linq;
using iSHARE.Models.DelegationEvidence;
using iSHARE.Models.DelegationMask;

namespace iSHARE.ServiceProvider.Api.Authorization
{
    public static class DelegationEvidenceExtensions
    {
        public static bool Permit(this DelegationEvidence evidence,
            IReadOnlyCollection<string> identifiers,
            IReadOnlyCollection<string> attributes,
            IReadOnlyCollection<string> actions,
            string serviceProvider)
        {
            if (DateTime.UtcNow < GetDateTimeFromTimestamp(evidence.NotBefore) || DateTime.UtcNow >= GetDateTimeFromTimestamp(evidence.NotOnOrAfter))
            {
                return false;
            }

            return evidence.PolicySets
                .SelectMany(policySet => policySet.Policies)
                .All(policy =>
                {
                    var resourcesPolicy = identifiers.All(a => policy.Target.Resource.Identifiers.Contains(a))
                             || policy.Target.Resource.Identifiers.Contains("*");
                    var attributesPolicy = attributes.All(a => policy.Target.Resource.Attributes.Contains(a))
                             || policy.Target.Resource.Attributes.Contains("*");
                    var actionsPolicy = actions.All(a => policy.Target.Actions.Contains(a))
                             || policy.Target.Actions.Contains("*");
                    var serviceProviderPolicy = true;
                    if (policy.Target.Environment != null)
                    {
                        serviceProviderPolicy = policy.Target.Environment.ServiceProviders.Contains(serviceProvider);
                    }
                    var permit = policy.Rules.Any(rule => rule.Effect == "Permit");

                    return permit
                           && resourcesPolicy
                           && attributesPolicy
                           && actionsPolicy
                           && serviceProviderPolicy;
                });
        }

        public static bool Permit(this DelegationEvidence evidence, string serviceProvider, DelegationRequestPolicySet customPolicies)
        {
            if (DateTime.UtcNow < GetDateTimeFromTimestamp(evidence.NotBefore) || DateTime.UtcNow >= GetDateTimeFromTimestamp(evidence.NotOnOrAfter))
            {
                return false;
            }

            return customPolicies.Policies.All(
                policy => evidence.PolicySets.Any(
                    x => x.Policies.Any(
                        y =>
                        {
                            var resourcesPolicy = y.Target.Resource.Identifiers.Any(z => policy.Target.Resource.Identifiers.Contains(z));
                            var attributesPolicy = y.Target.Resource.Attributes.Any(z => policy.Target.Resource.Attributes.Contains(z));
                            var actionsPolicy = y.Target.Actions.Any(z => policy.Target.Actions.Contains(z));
                            var permit = y.Rules.All(rule => rule.Effect == "Permit");
                            var serviceProviderPolicy = true;
                            if (y.Target.Environment != null)
                            {
                                serviceProviderPolicy = y.Target.Environment.ServiceProviders.Contains(serviceProvider);
                            }
                            return permit
                                   && resourcesPolicy
                                   && attributesPolicy
                                   && actionsPolicy
                                   && serviceProviderPolicy;
                        }

                        )
                    )
               );
        }


        private static DateTime GetDateTimeFromTimestamp(int timestamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
    }
}
