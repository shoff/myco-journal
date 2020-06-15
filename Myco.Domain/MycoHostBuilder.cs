namespace Myco.Domain
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Hosting.Internal;
    using Zatoichi.Common.Infrastructure.Configuration;
    using Zatoichi.Common.Infrastructure.Security;

    public class MycoHostBuilder : IHostBuilder
    {
        private readonly List<Action<IConfigurationBuilder>> configureHostConfigActions = new List<Action<IConfigurationBuilder>>();
        private IConfigurationRoot hostConfiguration;
        private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> configureAppConfigActions = new List<Action<HostBuilderContext, IConfigurationBuilder>>();
        private readonly List<Action<HostBuilderContext, IServiceCollection>> configureServicesActions = new List<Action<HostBuilderContext, IServiceCollection>>();
        private readonly List<IConfigureContainerAdapter> configureContainerActions = new List<IConfigureContainerAdapter>();
        private IServiceFactoryAdapter<MycoContainerBuilder> serviceProviderFactory = new MycoServiceFactoryAdapter(new MycoServiceProviderFactory());
        private bool hostBuilt;
        private IConfiguration appConfiguration;
        private HostBuilderContext hostBuilderContext;
        private HostingEnvironment hostingEnvironment;
        private IServiceProvider appServices;

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            this.configureHostConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            this.configureAppConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            this.configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            this.serviceProviderFactory = new MycoServiceFactoryAdapter((MycoServiceProviderFactory)factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            Func<HostBuilderContext, IServiceProviderFactory<MycoContainerBuilder>> func =
                (Func<HostBuilderContext, IServiceProviderFactory<MycoContainerBuilder>>)factory;
            this.serviceProviderFactory = new MycoServiceFactoryAdapter(() => this.hostBuilderContext, func);
            return this;
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            this.configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                                                                                                ?? throw new ArgumentNullException(nameof(configureDelegate))));
            return this;
        }

        public IHost Build()
        {
            if (this.hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }
            this.hostBuilt = true;

            BuildHostConfiguration();
            CreateHostingEnvironment();
            CreateHostBuilderContext();
            BuildAppConfiguration();
            CreateServiceProvider();

            return this.appServices.GetRequiredService<IHost>();
        }


        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        private void CreateHostingEnvironment()
        {
            this.hostingEnvironment = new HostingEnvironment()
            {
                ApplicationName = "MycoJournal",
                EnvironmentName = Environments.Production,
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory
            };
            this.hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(this.hostingEnvironment.ContentRootPath);
        }
        private void BuildHostConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(); // Make sure there's some default storage since there are no default providers

            foreach (var buildAction in this.configureHostConfigActions)
            {
                buildAction(configBuilder);
            }
            this.hostConfiguration = configBuilder.Build();
        }
        private void CreateHostBuilderContext()
        {
            this.hostBuilderContext = new HostBuilderContext(this.Properties)
            {
                HostingEnvironment = this.hostingEnvironment,
                Configuration = this.hostConfiguration
            };
        }
        private void BuildAppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(this.hostingEnvironment.ContentRootPath)
                .AddConfiguration(this.hostConfiguration, shouldDisposeConfiguration: true);

            foreach (var buildAction in this.configureAppConfigActions)
            {
                buildAction(this.hostBuilderContext, configBuilder);
            }
            this.appConfiguration = configBuilder.Build();
            this.hostBuilderContext.Configuration = this.appConfiguration;
        }
        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddSingleton<IHostingEnvironment>(this.hostingEnvironment);
#pragma warning restore CS0618 // Type or member is obsolete
            services.AddSingleton<IHostEnvironment>(this.hostingEnvironment);
            services.AddSingleton(this.hostBuilderContext);
            // register configuration as factory to make it dispose with the service provider
            services.AddSingleton(_ => this.appConfiguration);
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddSingleton<IApplicationLifetime>(s => (IApplicationLifetime)s.GetService<IHostApplicationLifetime>());
#pragma warning restore CS0618 // Type or member is obsolete
            services.AddSingleton<IHostApplicationLifetime, ApplicationLifetime>();
            services.AddSingleton<IHostLifetime, ConsoleLifetime>();
            services.AddSingleton<IHost, MycoHost>();
            services.AddOptions();
            services.AddLogging();

            foreach (var configureServicesAction in this.configureServicesActions)
            {
                configureServicesAction(this.hostBuilderContext, services);
            }

            var containerBuilder = this.serviceProviderFactory.CreateBuilder(services);

            foreach (var containerAction in this.configureContainerActions)
            {
                containerAction.ConfigureContainer(this.hostBuilderContext, containerBuilder);
            }

            this.appServices = this.serviceProviderFactory.CreateServiceProvider(containerBuilder);

            if (this.appServices == null)
            {
                throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
            }

            // resolve configuration explicitly once to mark it as resolved within the
            // service provider, ensuring it will be properly disposed with the provider
            _ = this.appServices.GetService<IConfiguration>();
        }
    }

    public class MycoContainerBuilder
    {
        public MycoContainerBuilder(IServiceCollection services) =>
            (this.Services) = (services);

        public IServiceCollection Services { get; }


        public void ConfigureServices(IConfiguration configuration)
        {
            this.Services.Configure<Crypt>(configuration.GetSection("Crypt"));
            this.Services.Configure<Journal>(configuration.GetSection("Journal"));
            this.Services.AddSingleton<IEncryptor, Encryptor>();
        }
    }
}