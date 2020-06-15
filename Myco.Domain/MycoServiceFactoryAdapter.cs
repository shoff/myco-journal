namespace Myco.Domain
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class MycoServiceFactoryAdapter : IServiceFactoryAdapter<MycoContainerBuilder>
    {
        private IServiceProviderFactory<MycoContainerBuilder> serviceProviderFactory;
        private readonly Func<HostBuilderContext> contextResolver;
        private readonly Func<HostBuilderContext, IServiceProviderFactory<MycoContainerBuilder>> factoryResolver;

        public MycoServiceFactoryAdapter(IServiceProviderFactory<MycoContainerBuilder> serviceProviderFactory)
        {
            this.serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        public MycoServiceFactoryAdapter(Func<HostBuilderContext> contextResolver, Func<HostBuilderContext, IServiceProviderFactory<MycoContainerBuilder>> factoryResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            this.factoryResolver = factoryResolver ?? throw new ArgumentNullException(nameof(factoryResolver));
        }

        public MycoContainerBuilder CreateBuilder(IServiceCollection services)
        {
            if (this.serviceProviderFactory == null)
            {
                this.serviceProviderFactory = this.factoryResolver(this.contextResolver());

                if (this.serviceProviderFactory == null)
                {
                    throw new InvalidOperationException("The resolver returned a null IServiceProviderFactory");
                }
            }
            return this.serviceProviderFactory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            if (this.serviceProviderFactory == null)
            {
                throw new InvalidOperationException("CreateBuilder must be called before CreateServiceProvider");
            }

            return this.serviceProviderFactory.CreateServiceProvider((MycoContainerBuilder)containerBuilder);
        }
    }
}