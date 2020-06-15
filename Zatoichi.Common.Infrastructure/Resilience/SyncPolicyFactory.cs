namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System;
    using System.IO;
    using ChaosMonkey.Guards;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Options;
    using Polly;

    public class SyncPolicyFactory : PolicyFactory, ISyncPolicyFactory
    {
        private readonly ISyncPolicy apiExecutionSyncPolicy;

        private readonly ISyncPolicy dbExecutionSyncPolicy;

        private readonly ISyncPolicy ioExecutionSyncPolicy;

        private readonly ISyncPolicy queueExecutionSyncPolicy;

        public SyncPolicyFactory(IOptions<PolicyOptions> policyOptions)
        {
            policyOptions = Guard.IsNotNull(policyOptions, "policyOptions");
            var val = Policy.Handle((Func<SqlException, bool>) (ex => this.IsTransientSqlException(ex)))
                .WaitAndRetry(policyOptions.Value.DbRetryCount,
                    attempt => TimeSpan.FromMilliseconds(policyOptions.Value.DbRetryBaseDelay * attempt));
            var circuitBreakerSqlPolicy = Policy
                .Handle((Func<SqlException, bool>) (ex => this.IsTransientSqlException(ex))).CircuitBreaker(
                    policyOptions.Value.DbCircuitBreakerErrorCount,
                    TimeSpan.FromMilliseconds(policyOptions.Value.DbCircuitBreakerDelay));
            this.dbExecutionSyncPolicy = val.Execute(() => circuitBreakerSqlPolicy);
            var val2 = Policy.Handle<Exception>().WaitAndRetry(policyOptions.Value.ApiRetryCount,
                attempt => TimeSpan.FromMilliseconds(policyOptions.Value.ApiRetryBaseDelay * attempt));
            var circuitBreakerApiPolicy = Policy.Handle<Exception>().CircuitBreaker(
                policyOptions.Value.ApiCircuitBreakerErrorCount,
                TimeSpan.FromMilliseconds(policyOptions.Value.ApiCircuitBreakerDelay));
            this.apiExecutionSyncPolicy = val2.Execute(() => circuitBreakerApiPolicy);
            var val3 = Policy.Handle<IOException>().WaitAndRetry(policyOptions.Value.QueueRetryCount,
                attempt => TimeSpan.FromMilliseconds(policyOptions.Value.QueueRetryBaseDelay * attempt));
            var circuitBreakerQueuePolicy = Policy.Handle<IOException>().CircuitBreaker(
                policyOptions.Value.QueueCircuitBreakerErrorCount,
                TimeSpan.FromMilliseconds(policyOptions.Value.QueueCircuitBreakerDelay));
            this.queueExecutionSyncPolicy = val3.Execute(() => circuitBreakerQueuePolicy);
            var val4 = Policy.Handle<Exception>().WaitAndRetry(policyOptions.Value.IORetryCount,
                attempt => TimeSpan.FromMilliseconds(policyOptions.Value.IORetryBaseDelay * attempt));
            var circuitBreakerIoPolicy = Policy.Handle<Exception>().CircuitBreaker(
                policyOptions.Value.IOCircuitBreakerErrorCount,
                TimeSpan.FromMilliseconds(policyOptions.Value.IOCircuitBreakerDelay));
            this.ioExecutionSyncPolicy = val4.Execute(() => circuitBreakerIoPolicy);
        }

        public ISyncPolicy GetIoExecutionSyncPolicy()
        {
            return this.ioExecutionSyncPolicy;
        }

        public ISyncPolicy GetDbExecutionSyncPolicy()
        {
            return this.dbExecutionSyncPolicy;
        }

        public ISyncPolicy GetApiExecutionSyncPolicy()
        {
            return this.apiExecutionSyncPolicy;
        }

        public ISyncPolicy GetQueueExecutionSyncPolicy()
        {
            return this.queueExecutionSyncPolicy;
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