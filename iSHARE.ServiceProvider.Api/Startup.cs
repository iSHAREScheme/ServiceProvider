using System.Reflection;
using iSHARE.Api;
using iSHARE.Api.Attributes;
using iSHARE.AuthorizationRegistry.Client;
using iSHARE.AzureKeyVaultClient;
using iSHARE.IdentityServer;
using iSHARE.IdentityServer.Data;
using iSHARE.ServiceProvider.Core;
using iSHARE.ServiceProvider.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iSHARE.ServiceProvider.Api
{
    public class Startup : iSHARE.Api.Startup
    {
        protected internal string IdentityServerSeedNamespace;
        protected internal Assembly IdentityServerSeedAssembly;
        protected internal string ServiceProviderSeedNamespace;
        protected internal Assembly ServiceProviderSeedAssembly;
        public IIdentityServerBuilder IdentityServerBuilder { get; private set; }


        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(env, configuration, loggerFactory)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            var idpOptions = services.AddSchemeOwnerIdentityProviderOptions(Configuration);
            services.ConfigureSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerServiceConsumerFilter>();
            });

            services.AddCore();
            services.AddServiceProviderAuthorization();
            services.AddAuthorizationRegistryClient(Configuration);
            services.AddDb(Configuration, HostingEnvironment, ServiceProviderSeedNamespace, ServiceProviderSeedAssembly);

            IdentityServerBuilder = services
                .AddIdentityServer(Configuration, HostingEnvironment, LoggerFactory)
                .AddPki(Configuration)
                .AddIdentityServerDb(Configuration, HostingEnvironment, IdentityServerSeedNamespace, IdentityServerSeedAssembly)
                .AddConsumer()
                .AddSchemeOwnerValidator(Configuration, HostingEnvironment)
                ;
            services.AddDigitalSigner(Configuration, HostingEnvironment, LoggerFactory);
            MvcCoreBuilder.AddAuthorization(opt =>
            {
                opt.AddPolicy(SpaConstants.SpaPolicy, policy => policy.RequireAssertion(_ => true));
            });
        }

        public override void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMigrations(Configuration, HostingEnvironment);
            app.UseIdentityServer() // this calls UseAuthentication
               .UseIdentityServerDb(Configuration, HostingEnvironment);

            base.Configure(app, loggerFactory); // this calls UseMvc, which needs to be called before UseAuthentication
        }
    }
}
