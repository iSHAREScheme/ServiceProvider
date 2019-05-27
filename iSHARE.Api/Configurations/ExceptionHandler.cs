using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using iSHARE.Configuration.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace iSHARE.Api.Configurations
{
    public static class ExceptionHandler
    {
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app, IHostingEnvironment hostingEnvironment)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(
                    async context =>
                    {
                        var errorDetailsOptions = (IOptionsSnapshot<ErrorDetailsOptions>)context.RequestServices.GetService(typeof(IOptionsSnapshot<ErrorDetailsOptions>));

                        var showDetails = !hostingEnvironment.IsLive() || (errorDetailsOptions.Value?.Show ?? false);
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        var exceptions = new List<Exception>();
                        if (ex != null)
                        {
                            exceptions.Add(ex.Error);
                            var innerException = ex.Error.InnerException;
                            while (innerException != null)
                            {
                                exceptions.Add(innerException);
                                innerException = innerException.InnerException;
                            }
                        }

                        var json = exceptions.Any() ? JsonConvert.SerializeObject(exceptions.Select(c => new
                        {
                            c.GetType().FullName,
                            c.Message,
                            StackTrace = showDetails ? c.StackTrace : null,
                            Source = showDetails ? c.Source : null,
                            Data = showDetails ? (c.Data.Any() ? c.Data.Cast<DictionaryEntry>()
                                         .Aggregate(new StringBuilder(), (s, x) => s.Append(x.Key + ":" + x.Value + "|"), s => s.ToString(0, s.Length - 1)) : null) : null
                        })) : @"{ ""Errors"" : ""An error occurred, if you are a developer please check the logs."" }";

                        await context.Response.WriteAsync(json);
                    });
            });



            if (hostingEnvironment.IsDevelopment() || hostingEnvironment.IsQa())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }
    }
}
