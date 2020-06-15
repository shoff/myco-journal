namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System;
    using ChaosMonkey.Guards;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.Wrap;

    public class AsyncPolicyFactory : PolicyFactory, IPolicyFactory
    {
        private readonly AsyncPolicyWrap apiExecutionPolicy;

        private readonly AsyncPolicyWrap dbExecutionPolicy;

        private readonly AsyncPolicyWrap queueExecutionPolicy;

        public AsyncPolicyFactory(IOptions<PolicyOptions> policyOptions)
        {
            Guard.IsNotNull(policyOptions, nameof(policyOptions));
            this.queueExecutionPolicy = Policy.Handle<InvalidOperationException>()
                .CircuitBreakerAsync(policyOptions.Value.QueueCircuitBreakerErrorCount,
                    TimeSpan.FromMilliseconds(policyOptions.Value.QueueCircuitBreakerDelay)).WrapAsync(Policy
                    .Handle<InvalidOperationException>().WaitAndRetryAsync(policyOptions.Value.QueueRetryCount,
                        attempt => TimeSpan.FromMilliseconds(policyOptions.Value.QueueRetryBaseDelay * attempt)));
            this.dbExecutionPolicy = Policy.Handle((Func<SqlException, bool>) (ex => this.IsTransientSqlException(ex)))
                .CircuitBreakerAsync(policyOptions.Value.DbCircuitBreakerErrorCount,
                    TimeSpan.FromMilliseconds(policyOptions.Value.ApiCircuitBreakerDelay)).WrapAsync(Policy
                    .Handle((Func<SqlException, bool>) (ex => this.IsTransientSqlException(ex)))
                    .WaitAndRetryAsync(policyOptions.Value.DbRetryCount,
                        attempt => TimeSpan.FromMilliseconds(policyOptions.Value.DbRetryBaseDelay * attempt)));
            this.apiExecutionPolicy = Policy.Handle((Func<Exception, bool>) IsTransientException)
                .CircuitBreakerAsync(policyOptions.Value.ApiCircuitBreakerErrorCount,
                    TimeSpan.FromMilliseconds(policyOptions.Value.ApiCircuitBreakerDelay)).WrapAsync(
                    Policy.Handle((Func<Exception, bool>) IsTransientException)
                        .WaitAndRetryAsync(policyOptions.Value.ApiRetryCount,
                            attempt => TimeSpan.FromMilliseconds(policyOptions.Value.ApiRetryBaseDelay * attempt)));
        }

        public IAsyncPolicy GetAsyncApiPolicy()
        {
            return this.apiExecutionPolicy;
        }

        public IAsyncPolicy GetDbExecutionPolicy()
        {
            return this.dbExecutionPolicy;
        }

        public IAsyncPolicy GetQueueExecutionPolicy()
        {
            return this.queueExecutionPolicy;
        }

        public class DefaultOptions : IOptions<PolicyOptions>
        {
            public DefaultOptions()
            {
                this.Value = new PolicyOptions
                {
                    ApiCircuitBreakerDelay = 1000,
                    ApiCircuitBreakerErrorCount = 3,
                    ApiRetryBaseDelay = 1000,
                    ApiRetryCount = 4,
                    DbCircuitBreakerDelay = 1000,
                    DbCircuitBreakerErrorCount = 3,
                    DbRetryBaseDelay = 1000,
                    DbRetryCount = 4,
                    QueueCircuitBreakerDelay = 1000,
                    QueueCircuitBreakerErrorCount = 3,
                    QueueRetryBaseDelay = 1000,
                    QueueRetryCount = 4
                };
            }

            public PolicyOptions Value { get; }
        }
    }
}