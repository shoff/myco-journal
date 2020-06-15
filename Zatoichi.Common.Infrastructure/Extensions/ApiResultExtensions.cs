namespace Zatoichi.Common.Infrastructure.Extensions
{
    using System;
    using System.Net;
    using ChaosMonkey.Guards;
    using Services;

    public static class ApiResultExtensions
    {
        public static IApiResult NotFoundResult()
        {
            return new ApiResult(HttpStatusCode.NotFound, null, false, null,
                "The requested resource could not be found.");
        }

        public static IApiResult<TData> NotFoundResult<TData>()
        {
            return new ApiResult<TData>(HttpStatusCode.NotFound, default, false, null,
                "The requested resource could not be found.");
        }

        public static IApiResult InternalServerErrorResult(Exception exception)
        {
            Guard.IsNotNull(exception, nameof(exception));
            return new ApiResult(HttpStatusCode.InternalServerError, null, false, null,
                exception.Message, exception);
        }

        public static IApiResult<TData> InternalServerErrorResult<TData>(Exception exception)
        {
            Guard.IsNotNull(exception, nameof(exception));
            return new ApiResult<TData>(HttpStatusCode.InternalServerError, default, false, null,
                exception.Message, exception);
        }

        public static IApiResult OkResult()
        {
            return new ApiResult(HttpStatusCode.OK, null);
        }
    }
}