namespace Zatoichi.Common.Infrastructure.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using ChaosMonkey.Guards;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Serilog;
    using Swagger;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCore(this IApplicationBuilder app,
            IWebHostEnvironment env, IConfiguration configuration, ILogger logger)
        {
            Guard.IsNotNull(app, nameof(app));
            Guard.IsNotNull(env, nameof(env));
            Guard.IsNotNull(configuration, nameof(configuration));

            using var serviceScope = app.ApplicationServices.CreateScope();
            var service = serviceScope.ServiceProvider.GetService<IOptionsSnapshot<AspnetCoreOptions>>();

            if (service.Value.UseAngularFolder)
            {
                AddAngularFolder(app, logger);
            }

            if (service.Value.UseForwardedHeaders)
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor 
                                       | ForwardedHeaders.XForwardedProto
                });
            }

            if (service.Value.UseCors
                && service.Value.Cors.Origins.Count > 0
                && service.Value.Cors.Methods.Count > 0)
            {
                app.UseCors(builder =>
                {
                    builder.WithHeaders(service.Value.Cors.Headers.ToArray());
                    builder.WithOrigins(service.Value.Cors.Origins.ToArray());
                    builder.WithMethods(service.Value.Cors.Methods.ToArray());
                });
            }

            app.UseRouting();
            app.UseRequestLocalization();
            
            if (service?.Value != null)
            {
                app.UseCoreSwagger(service.Value);
            }
            else
            {
                app.UseCoreSwagger();
            }

            if (service.Value.UseStaticFiles)
            {
                app.UseStaticFiles();
            }

            if (!env.IsDevelopment() && !env.IsLocal())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSerilogRequestLogging();
            }

            if (service.Value.UseCookiePolicy)
            {
                app.UseCookiePolicy(new CookiePolicyOptions
                {
                    MinimumSameSitePolicy = (SameSiteMode)service.Value.SafeSiteMode
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            return app;
        }

        private static void AddAngularFolder(IApplicationBuilder app, ILogger logger)
        {
            app.UseDefaultFiles();

            _ = app.Use(async (context, next) =>
            {
                try
                {
                    await next().ConfigureAwait(false);
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next().ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
            });
        }
    }
}