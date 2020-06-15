namespace Zatoichi.Common.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using Attributes;
    using ChaosMonkey.Guards;
    using Configuration;
    using Extensions.Identity;
    using Extensions.Swagger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using CorsOptions = Configuration.CorsOptions;

    public static class ServiceCollectionExtensions
    {
        internal const string BEARER = "Bearer";
        internal const string DEFAULT_CULTURE = "en-US";
        internal const string ASPNET_CORE_OPTIONS = "AspnetCoreOptions";

        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration,
            Action<AspnetCoreOptions> configureOptions = null)
        {
            services.Configure<AspnetCoreOptions>(configuration.GetSection(ASPNET_CORE_OPTIONS),
                binder => binder.BindNonPublicProperties = true);

            if (configureOptions != null)
            {
                services.PostConfigure(configureOptions);
            }

            var aspnetCoreOptions = services.BuildServiceProvider().GetService<IOptionsSnapshot<AspnetCoreOptions>>()
                ?.Value;
            if (aspnetCoreOptions == null)
            {
                aspnetCoreOptions = new AspnetCoreOptions
                {
                    Cors = new CorsOptions()
                };
                aspnetCoreOptions.SupportedCultures.Add(DEFAULT_CULTURE);
            }

            services.AddLocalizationSupport(aspnetCoreOptions);
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder(BEARER).RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                aspnetCoreOptions?.MvcOptions?.Invoke(options);
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddJwtAuthentication(aspnetCoreOptions.Jwt);
            services.AddSwagger(aspnetCoreOptions.ClientSecurity);
            services.AddCorsSupport(aspnetCoreOptions);
            return services;
        }

        private static void AddLocalizationSupport(this IServiceCollection services,
            AspnetCoreOptions aspnetCoreOptions)
        {
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            if (aspnetCoreOptions.SupportedCultures.Any())
            {
                services.Configure<RequestLocalizationOptions>(options =>
                {
                    var distinctCultureNames = aspnetCoreOptions.SupportedCultures
                        .Union(new[] {CultureInfo.CurrentCulture.Name})
                        .Distinct(StringComparer.InvariantCultureIgnoreCase);

                    var supportedCultures = distinctCultureNames.Select(x => new CultureInfo(x)).ToList();
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
            }
        }

        private static void AddCorsSupport(this IServiceCollection services, AspnetCoreOptions aspnetCoreOptions)
        {
            services.AddCors(delegate(Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions options)
            {
                options.AddDefaultPolicy(delegate(CorsPolicyBuilder builder)
                {
                    if (aspnetCoreOptions.Cors.Origins.Any())
                    {
                        builder.WithOrigins(aspnetCoreOptions.Cors.Origins.ToArray());
                    }

                    if (aspnetCoreOptions.Cors.Headers.Any())
                    {
                        builder.WithHeaders(aspnetCoreOptions.Cors.Headers.ToArray());
                    }

                    if (aspnetCoreOptions.Cors.Methods.Any())
                    {
                        builder.WithMethods(aspnetCoreOptions.Cors.Methods.ToArray());
                    }
                });
            });
        }

        public static void RegisterAllForAssembly(this IServiceCollection services, Assembly assembly)
        {
            Guard.IsNotNull(assembly, nameof(assembly));
            var transientTypes = assembly.GetTypes().Where(a => a.GetCustomAttribute<TransientAttribute>() != null)
                .Select(t => t).ToList();
            var singletonTypes = assembly.GetTypes().Where(a => a.GetCustomAttribute<SingletonAttribute>() != null)
                .Select(t => t).ToList();
            var scopedTypes = assembly.GetTypes().Where(a => a.GetCustomAttribute<ScopedAttribute>() != null)
                .Select(t => t).ToList();

            foreach (var transient in transientTypes)
            {
                var attribute = transient.GetCustomAttribute<TransientAttribute>();
                var serviceType = attribute.ServiceType;
                if (serviceType != null)
                {
                    services.AddTransient(serviceType, transient);
                }
                else
                {
                    services.AddTransient(transient);
                }
            }


            foreach (var singleton in singletonTypes)
            {
                var attribute = singleton.GetCustomAttribute<SingletonAttribute>();
                var serviceType = attribute.ServiceType;
                if (serviceType != null)
                {
                    services.AddSingleton(serviceType, singleton);
                }
                else
                {
                    services.AddSingleton(singleton);
                }
            }


            foreach (var scoped in scopedTypes)
            {
                var attribute = scoped.GetCustomAttribute<ScopedAttribute>();
                var serviceType = attribute.ServiceType;
                if (serviceType != null)
                {
                    services.AddScoped(serviceType, scoped);
                }
                else
                {
                    services.AddScoped(scoped);
                }
            }
        }
    }
}