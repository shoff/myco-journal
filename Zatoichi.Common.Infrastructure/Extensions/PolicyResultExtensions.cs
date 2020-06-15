namespace Zatoichi.Common.Infrastructure.Extensions
{
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.Logging;
    using Polly;

    public static class PolicyResultExtensions
    {
        public static void LogResult(this ILogger logger, PolicyResult policyResult, string handler)
        {
            Guard.IsNotNull(logger, nameof(logger));
            Guard.IsNotNull(policyResult, nameof(policyResult));
            Guard.IsNotNullOrWhitespace(handler, nameof(handler));

            if (policyResult.Outcome == OutcomeType.Failure && policyResult.FinalException != null)
            {
                logger.LogError(policyResult.FinalException, policyResult.FinalException.Message);
            }
            else if (policyResult.Outcome == OutcomeType.Failure)
            {
                logger.LogError($"Unknown error executing {handler}.");
            }

            logger.LogInformation($"{handler} completed successfully.");
        }
    }
}