namespace Zatoichi.Common.Infrastructure.Resilience
{
    using Polly;

    public interface ISyncPolicyFactory
    {
        ISyncPolicy GetDbExecutionSyncPolicy();

        ISyncPolicy GetApiExecutionSyncPolicy();

        ISyncPolicy GetQueueExecutionSyncPolicy();

        ISyncPolicy GetIoExecutionSyncPolicy();
    }
}