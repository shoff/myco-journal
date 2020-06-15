namespace Zatoichi.Common.Infrastructure.Resilience
{
    using ChaosMonkey.Guards;
    using Polly;

    public class ExecutionPolicies : IExecutionPolicies
    {
        private readonly IPolicyFactory policyFactory;

        public ExecutionPolicies(IPolicyFactory policyFactory)
        {
            Guard.IsNotNull(policyFactory, nameof(policyFactory));
            this.policyFactory = policyFactory;
        }

        public IAsyncPolicy DbExecutionPolicy => (AsyncPolicy) this.policyFactory.GetDbExecutionPolicy();

        public IAsyncPolicy ApiExecutionPolicy => (AsyncPolicy) this.policyFactory.GetAsyncApiPolicy();

        public IAsyncPolicy QueueExecutionPolicy => (AsyncPolicy) this.policyFactory.GetQueueExecutionPolicy();
    }
}