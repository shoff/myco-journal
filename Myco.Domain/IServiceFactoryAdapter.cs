namespace Myco.Domain
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    internal interface IServiceFactoryAdapter<out TContainerBuilder>
    {
        TContainerBuilder CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}