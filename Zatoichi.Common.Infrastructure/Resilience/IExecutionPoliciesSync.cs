namespace Zatoichi.Common.Infrastructure.Resilience
{
    using Polly;

    public interface IExecutionPoliciesSync
    {
        ISyncPolicy DbExecutionSyncPolicy { get; }

        ISyncPolicy ApiExecutionSyncPolicy { get; }

        ISyncPolicy QueueExecutionSyncPolicy { get; }

        ISyncPolicy IoExecuteSyncPolicy { get; }
    }
}