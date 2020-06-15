namespace Zatoichi.Common.Infrastructure.Resilience
{
    using ChaosMonkey.Guards;
    using Polly;

    public class ExecutionPoliciesSync : IExecutionPoliciesSync
    {
        private readonly ISyncPolicyFactory syncPolicyFactory;

        public ExecutionPoliciesSync(ISyncPolicyFactory syncPolicyFactory)
        {
            Guard.IsNotNull(syncPolicyFactory, nameof(syncPolicyFactory));
            this.syncPolicyFactory = syncPolicyFactory;
        }

        public ISyncPolicy DbExecutionSyncPolicy => (Policy) this.syncPolicyFactory.GetDbExecutionSyncPolicy();

        public ISyncPolicy ApiExecutionSyncPolicy => (Policy) this.syncPolicyFactory.GetApiExecutionSyncPolicy();

        public ISyncPolicy QueueExecutionSyncPolicy =>
            (Policy) this.syncPolicyFactory.GetQueueExecutionSyncPolicy();

        public ISyncPolicy IoExecuteSyncPolicy => (Policy) this.syncPolicyFactory.GetIoExecutionSyncPolicy();
    }
}