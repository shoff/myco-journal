namespace Zatoichi.Common.Infrastructure.Resilience
{
    using Polly;

    public interface IPolicyFactory
    {
        IAsyncPolicy GetAsyncApiPolicy();

        IAsyncPolicy GetDbExecutionPolicy();

        IAsyncPolicy GetQueueExecutionPolicy();
    }
}