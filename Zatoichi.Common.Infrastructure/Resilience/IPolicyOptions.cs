namespace Zatoichi.Common.Infrastructure.Resilience
{
    public interface IPolicyOptions
    {
        int DbCircuitBreakerErrorCount { get; set; }

        int DbCircuitBreakerDelay { get; set; }

        int DbRetryCount { get; set; }

        int DbRetryBaseDelay { get; set; }

        int ApiCircuitBreakerErrorCount { get; set; }

        int ApiCircuitBreakerDelay { get; set; }

        int ApiRetryCount { get; set; }

        int ApiRetryBaseDelay { get; set; }

        int QueueCircuitBreakerErrorCount { get; set; }

        int QueueCircuitBreakerDelay { get; set; }

        int QueueRetryCount { get; set; }

        int QueueRetryBaseDelay { get; set; }

        int IOCircuitBreakerErrorCount { get; set; }

        int IOCircuitBreakerDelay { get; set; }

        int IORetryCount { get; set; }

        int IORetryBaseDelay { get; set; }
    }
}