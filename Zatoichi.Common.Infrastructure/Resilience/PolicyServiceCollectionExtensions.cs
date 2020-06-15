namespace Zatoichi.Common.Infrastructure.Resilience
{
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class PolicyServiceCollectionExtensions
    {
        public static IServiceCollection AddResilience(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));
            services.AddResilience(new AsyncPolicyFactory.DefaultOptions());
            return services;
        }

        public static IServiceCollection AddResilience(this IServiceCollection services,
            IOptions<PolicyOptions> policyOptions)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(policyOptions, nameof(policyOptions));
            services.TryAdd(ServiceDescriptor.Singleton(policyOptions));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IPolicyFactory), typeof(AsyncPolicyFactory)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ISyncPolicyFactory), typeof(SyncPolicyFactory)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IExecutionPolicies), typeof(ExecutionPolicies)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IExecutionPoliciesSync), typeof(ExecutionPoliciesSync)));
            return services;
        }
    }
}