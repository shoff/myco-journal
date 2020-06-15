namespace Zatoichi.Common.Infrastructure.Resilience
{
    using Polly;

    public interface ITypedPolicyFactory
    {
        IAsyncPolicy<TResult> GetAsyncApiPolicy<TResult>();

        IAsyncPolicy<TResult> GetDbExecutionPolicy<TResult>();

        IAsyncPolicy<TResult> GetQueueExecutionPolicy<TResult>();
    }
}