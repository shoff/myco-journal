namespace Zatoichi.Common.Infrastructure.Extensions
{
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.Hosting;

    public static class HostEnvironmentExtensions
    {
        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        {
            Guard.IsNotNull(hostEnvironment, nameof(hostEnvironment));
            return hostEnvironment.IsEnvironment("Local");
        }
    }
}