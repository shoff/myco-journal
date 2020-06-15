namespace Zatoichi.Common.Infrastructure.Extensions.Swagger
{
    using System.Collections.Generic;
    using System.Reflection;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.OpenApi.Models;

    public static class SwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseCoreSwagger(this IApplicationBuilder app,
            AspnetCoreOptions aspnetCoreOptions = null)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            string assemblyProduct;
            string version;
            if (entryAssembly != null)
            {
                assemblyProduct = entryAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
                version = $"V{entryAssembly.GetName().Version!.Major}";
            }
            else
            {
                assemblyProduct = ".Net Core API";
                version = "1.0.0.0";
            }

            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                        {new OpenApiServer {Url = $"{httpReq.Scheme}://{httpReq.Host.Value}"}};
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = assemblyProduct;
                options.DisplayRequestDuration();
                if (aspnetCoreOptions?.ClientSecurity != null)
                {
                    options.OAuthClientId(aspnetCoreOptions.ClientSecurity.ClientId);
                    options.OAuthClientSecret(aspnetCoreOptions.ClientSecurity.ClientSecret);
                }

                options.SwaggerEndpoint("/swagger/" + version + "/swagger.json", version);
            });
            app.UseReDoc(options =>
            {
                options.SpecUrl = "/swagger/" + version + "/swagger.json";
                options.DocumentTitle = assemblyProduct;
            });
            return app;
        }
    }
}