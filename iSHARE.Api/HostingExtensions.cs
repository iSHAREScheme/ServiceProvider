using System;
using iSHARE.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace iSHARE.Api
{
    public static class HostingExtensions
    {
        public static IWebHostBuilder UseDefaultWebHostOptions<TStartup>(this IWebHostBuilder builder)
            where TStartup : class
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var showDetailedErrors =
                !string.Equals(env, Environments.Test, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(env, Environments.Live, StringComparison.OrdinalIgnoreCase)
                ? "true" : "false";

            var detailedErrorsFromEnvironment = GetDetailedErrorsFromEnvironment();

            if (!string.IsNullOrEmpty(detailedErrorsFromEnvironment))
            {
                showDetailedErrors = detailedErrorsFromEnvironment;
            }

            return builder
                .UseAzureAppServices()
                .UseApplicationInsights()
                .UseStartup<TStartup>()
                .UseSetting("detailedErrors", showDetailedErrors)
                ;
        }

        private static string GetDetailedErrorsFromEnvironment()
            => Environment.GetEnvironmentVariable("ASPNETCORE_DETAILEDERRORS");
    }
}
