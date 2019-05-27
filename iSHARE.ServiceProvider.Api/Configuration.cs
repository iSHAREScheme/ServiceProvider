using iSHARE.Configuration;
using iSHARE.ServiceProvider.Api.Authorization;
using iSHARE.ServiceProvider.Core.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace iSHARE.ServiceProvider.Api
{
    public static class Configuration
    {
        public static void AddServiceProviderAuthorization(this IServiceCollection services)
        {
            services.AddTransient<DelegationEvidenceAuthorizationService>();
            services.AddTransient<DenyAuthorizationService>();
            services.AddTransient<ClientAssertionAuthorizationService>();
            services.AddTransient<IResourceAuthorizationService>(srv =>
            {
                var context = srv.GetRequiredService<IHttpContextAccessor>().HttpContext;
                if (context.Request.Headers.ContainsKey("delegation_evidence"))
                {
                    return srv.GetRequiredService<DelegationEvidenceAuthorizationService>();
                }
                if (context.Request.Headers.ContainsKey("client_assertion"))
                {
                    return srv.GetRequiredService<ClientAssertionAuthorizationService>();
                }
                return srv.GetRequiredService<DenyAuthorizationService>();
            });
            services.AddTransientDecorator<IContainersService, AuthorizationContainersServiceDecorator>();
        }
    }
}
