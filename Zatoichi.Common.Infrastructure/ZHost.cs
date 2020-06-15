namespace Zatoichi.Common.Infrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Extensions.Logging;
    using Serilog.Sinks.Elasticsearch;

    [ExcludeFromCodeCoverage]
    public static class ZHost
    {
        /*
         https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1
         The CreateDefaultBuilder method:

            Sets the content root to the path returned by GetCurrentDirectory.
            Loads host configuration from:
                Environment variables prefixed with DOTNET_.
                Command-line arguments.
            Loads app configuration from:
                appsettings.json.
                appsettings.{Environment}.json.
                Secret Manager when the app runs in the Development environment.
                Environment variables.
                Command-line arguments.
            Adds the following logging providers:
                Console
                Debug
                EventSource
                EventLog (only when running on Windows)
            Enables scope validation and dependency validation when the environment is Development.

        The ConfigureWebHostDefaults method:

            Loads host configuration from environment variables prefixed with ASPNETCORE_.
            Sets Kestrel server as the web server and configures it using the app's hosting configuration providers. 
            For the Kestrel server's default options, see Kestrel web server implementation in ASP.NET Core.
            Adds Host Filtering middleware.
            Adds Forwarded Headers middleware if ASPNETCORE_FORWARDEDHEADERS_ENABLED equals true.
            Enables IIS integration. For the IIS default options, see Host ASP.NET Core on Windows with IIS.

        Settings for web apps

            Some host settings apply only to HTTP workloads. By default, environment variables used to 
            configure these settings can have a DOTNET_ or ASPNETCORE_ prefix.

            Extension methods on IWebHostBuilder are available for these settings. Code samples that 
            show how to call the extension methods assume webBuilder is an instance of IWebHostBuilder, as in the following example:

        https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1#endpoint-configuration
         */
         
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static ILogger logger;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private static readonly LoggerProviderCollection providers = new LoggerProviderCollection();

        internal static string HostingEnvironment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process) ??
            "Production";

        internal static string ElasticSearch =>
            Environment.GetEnvironmentVariable("ELASTICSEARCH_HOST", EnvironmentVariableTarget.Process) ??
            "https://elastic:H%40cker33@es.unitedstateskendo.com:9200";

        private static IConfigurationRoot configuration;

        public static IHostBuilder CreateConfiguredHost<TStartup>(string[] args = null) where TStartup : class
        {
            configuration = CreateConfiguration();
            bool.TryParse(configuration.GetSection("AspnetCoreOptions")["UseIIS"],
                out bool useIIs);

            return useIIs ? CreateBuilderForIIS<TStartup>(args)
                : CreateBuilderForKestrel<TStartup>(args);
        }


        public static IHostBuilder CreateBuilderForKestrel<TStartup>(string[] args) where TStartup : class
        {
            var configuration = CreateConfiguration();
            bool.TryParse(configuration.GetSection("AspnetCoreOptions")["CaptureStartupErrors"],
                out bool captureStartupErrors);

            string useUrls = configuration.GetSection("AspnetCoreOptions")["UseUrls"];

            var host = Host.CreateDefaultBuilder(args)
                .UseEnvironment(HostingEnvironment)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    webBuilder.UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                    });
                    if (!string.IsNullOrWhiteSpace(useUrls))
                    {
                        webBuilder.UseUrls(useUrls);
                    }
                    webBuilder.CaptureStartupErrors(captureStartupErrors);
                }).UseSerilog(BuildLogger())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile($"appsettings.{HostingEnvironment}.json");
                    config.AddJsonFile($"secrets.{HostingEnvironment}.json");
                    config.AddCommandLine(args);
                });
            return host;
        }

        public static IHostBuilder CreateBuilderForIIS<TStartup>(string[] args) where TStartup : class
        {
            var configuration = CreateConfiguration();
            bool.TryParse(configuration.GetSection("AspnetCoreOptions")["CaptureStartupErrors"],
                out bool captureStartupErrors);
            string useUrls = configuration.GetSection("AspnetCoreOptions")["UseUrls"];

            var host = Host.CreateDefaultBuilder(args)
                .UseEnvironment(HostingEnvironment)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    webBuilder.UseIIS();
                    webBuilder.CaptureStartupErrors(captureStartupErrors);
                    if (!string.IsNullOrWhiteSpace(useUrls))
                    {
                        webBuilder.UseUrls(useUrls);
                    }
                }).UseSerilog(BuildLogger())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile($"appsettings.{HostingEnvironment}.json");
                    config.AddJsonFile($"secrets.{HostingEnvironment}.json");
                    config.AddCommandLine(args);
                });

            return host;
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{HostingEnvironment}.json")
                .AddJsonFile($"secrets.{HostingEnvironment}.json");

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }

        internal static ILogger BuildLogger()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyProduct = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

            var log = new LoggerConfiguration();
#if DEBUG
                log.WriteTo.ColoredConsole()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .WriteTo.RollingFile("logs/log-{Date}.log");
#else
            log.MinimumLevel.Information()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                        new Uri(ElasticSearch))
                {
                    AutoRegisterTemplate = true,
                    DetectElasticsearchVersion = true,
                    MinimumLogEventLevel = LogEventLevel.Information
                }
                );
#endif
            log.Enrich.FromLogContext()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithProperty("ApplicationName", assemblyProduct ?? ".Net API")
                .WriteTo.Providers(providers);
            logger = log.CreateLogger();
            return logger;
        }
    }
}