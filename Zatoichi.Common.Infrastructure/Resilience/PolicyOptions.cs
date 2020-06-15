namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class PolicyOptions : IPolicyOptions
    {
        public int DbCircuitBreakerErrorCount { get; set; }

        public int DbCircuitBreakerDelay { get; set; }

        public int DbRetryCount { get; set; }

        public int DbRetryBaseDelay { get; set; }

        public int ApiCircuitBreakerErrorCount { get; set; }

        public int ApiCircuitBreakerDelay { get; set; }

        public int ApiRetryCount { get; set; }

        public int ApiRetryBaseDelay { get; set; }

        public int QueueCircuitBreakerErrorCount { get; set; }

        public int QueueCircuitBreakerDelay { get; set; }

        public int QueueRetryCount { get; set; }

        public int QueueRetryBaseDelay { get; set; }

        public int IOCircuitBreakerErrorCount { get; set; }

        public int IOCircuitBreakerDelay { get; set; }

        public int IORetryCount { get; set; }

        public int IORetryBaseDelay { get; set; }
    }
}