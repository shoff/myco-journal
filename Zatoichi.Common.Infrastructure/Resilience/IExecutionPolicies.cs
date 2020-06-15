namespace Zatoichi.Common.Infrastructure.Resilience
{
    using Polly;

    public interface IExecutionPolicies
    {
        IAsyncPolicy DbExecutionPolicy { get; }

        IAsyncPolicy ApiExecutionPolicy { get; }

        IAsyncPolicy QueueExecutionPolicy { get; }
    }
}