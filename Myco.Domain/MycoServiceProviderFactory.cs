namespace Myco.Domain
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class MycoServiceProviderFactory : IServiceProviderFactory<MycoContainerBuilder>
    {
        public MycoContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return new MycoContainerBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(MycoContainerBuilder containerBuilder)
        {
            return containerBuilder.Services.BuildServiceProvider();
        }
    }
}