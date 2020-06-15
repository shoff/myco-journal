namespace Zatoichi.Common.Infrastructure.Extensions.Swagger
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Configuration;
    using Infrastructure.Swagger;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.OpenApi.Models;

    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services,
            ClientSecurityOptions clientSecurityOptions)
        {
            return services.AddSwaggerGen(options =>
            {
                var assembly = Assembly.GetEntryAssembly();
                var assemblyProduct = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? ".Net API";
                var assemblyDescription = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ??
                                          ".Net API";

                options.DescribeAllParametersInCamelCase();
                options.EnableAnnotations();

                // Add the XML comment file for entry assembly, so its contents can be displayed.
                foreach (var xmlFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml",
                    SearchOption.TopDirectoryOnly))
                {
                    options.IncludeXmlComments(xmlFile, true);
                }

                // Add Document and Operation filters
                options.DocumentFilter<HideDocumentFilter>();
                options.OperationFilter<ForbiddenResponseOperationFilter>();
                options.OperationFilter<UnauthorizedOperationFilter>();

                // Get OpenIdConnectConfiguration if jwtBearerOptions.Authority provided
                var jwtBearerOptions = services.BuildServiceProvider()
                    .GetService<IOptionsSnapshot<JwtBearerOptions>>()
                    .Get(JwtBearerDefaults.AuthenticationScheme);

                if (!string.IsNullOrWhiteSpace(jwtBearerOptions.Authority))
                {
                    var openIdConnectConfiguration = new OpenIdConnectConfiguration();
                    try
                    {
                        openIdConnectConfiguration = jwtBearerOptions.ConfigurationManager
                            .GetConfigurationAsync(CancellationToken.None).Result;
                    }
                    catch (Exception ex)
                    {
                        var logger = services.BuildServiceProvider().GetService<ILoggerFactory>()
                            .CreateLogger(nameof(SwaggerServiceCollectionExtensions));
                        logger?.LogError(ex, "Error retrieving IdentityServer configuration.");
                    }

                    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = openIdConnectConfiguration.AuthorizationEndpoint != null
                                    ? new Uri(openIdConnectConfiguration.AuthorizationEndpoint) : null,
                                TokenUrl = openIdConnectConfiguration.TokenEndpoint != null
                                    ? new Uri(openIdConnectConfiguration.TokenEndpoint) : null,
                                Scopes = clientSecurityOptions.Scopes
                            }
                        }
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                    {Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme}
                            },
                            clientSecurityOptions.Scopes.Keys.ToArray()
                        }
                    });
                }

                var version = $"V{assembly?.GetName().Version.Major}";
                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = assemblyProduct,
                    Description = assemblyDescription,
                    Version = version,
                    Contact = new OpenApiContact
                    {
                        Name = "MonkeyButt Development Team", Email = "development@jigeiko.com",
                        Url = new Uri(
                            "https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://github.com/OAI/OpenAPI-Specification/blob/master/LICENSE")
                    }
                });
            });
        }
    }
}