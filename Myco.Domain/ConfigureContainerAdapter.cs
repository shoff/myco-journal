namespace Myco.Domain
{
    using System;
    using Microsoft.Extensions.Hosting;

    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private readonly Action<HostBuilderContext, TContainerBuilder> action;

        public ConfigureContainerAdapter(Action<HostBuilderContext, TContainerBuilder> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder)
        {
            this.action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}