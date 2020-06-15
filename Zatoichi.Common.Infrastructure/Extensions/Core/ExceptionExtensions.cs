namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System;
    using System.Collections.Generic;
    using ChaosMonkey.Guards;

    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            Guard.IsNotNull(ex, "ex");
            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            } while (innerException != null);
        }
    }
}