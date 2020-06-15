namespace Myco.Domain
{
    using Microsoft.Extensions.Hosting;

    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder);
    }
}