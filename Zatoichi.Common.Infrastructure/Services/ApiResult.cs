namespace Zatoichi.Common.Infrastructure.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using Extensions;

    /// <summary>
    ///     Encapsulates the result of a request to an Api endpoint.
    /// </summary>
    public class ApiResult : IApiResult
    {
        public ApiResult(
            HttpStatusCode statusCode,
            object data,
            bool? isSuccessStatusCode = null,
            string location = null,
            string message = null,
            Exception exceptionObject = null)
        {
            this.StatusCode = statusCode;
            this.Data = data;
            this.ExceptionObject = exceptionObject;
            this.IsSuccessStatusCode = isSuccessStatusCode ?? statusCode.IsSuccessStatusCode();
            this.Location = location;
            if (string.IsNullOrWhiteSpace(location) &&
                (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.Moved))
            {
                this.Location = "http://127.0.0.1/"; // not accessible
            }
            else
            {
                this.Location = location;
            }

            if (string.IsNullOrWhiteSpace(message) && exceptionObject != null
                                                   && !string.IsNullOrWhiteSpace(exceptionObject.Message) &&
                                                   (statusCode == HttpStatusCode.BadRequest ||
                                                    statusCode == HttpStatusCode.Conflict))
            {
                this.Message = exceptionObject.Message;
            }
            else
            {
                this.Message = message;
            }
        }

        /// <summary>
        ///     Convenience property to check the http status code result
        /// </summary>
        public bool IsSuccessStatusCode { get; }

        /// <summary>
        ///     Any exception thrown while requesting the result
        /// </summary>
        public Exception ExceptionObject { get; }

        /// <summary>
        ///     The data returned from the api request
        /// </summary>
        public object Data { get; }

        /// <summary>
        ///     The http status code from the http request
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        ///     If the request created an entity then the hypermedia location of the created
        ///     entity should be set here.
        ///     example: <code>http://localhost/dog/1</code>
        /// </summary>
        public string Location { get; }

        /// <summary>
        ///     Additional meta data as needed by the result. This should be used to set
        ///     messages for the client when more than one situation can result in the same
        ///     status code, but the client might want more information to correct the issue.
        /// </summary>
        public string Message { get; }
    }

    /// <summary>
    ///     Encapsulates the result of a request to an Api endpoint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class ApiResult<T> : IApiResult<T>
    {
        public ApiResult(
            HttpStatusCode statusCode,
            T data,
            bool? isSuccessStatusCode = null,
            string location = null,
            string message = null,
            Exception exceptionObject = null)
        {
            this.StatusCode = statusCode;
            this.Data = data;
            this.ExceptionObject = exceptionObject;
            this.IsSuccessStatusCode = isSuccessStatusCode ?? statusCode.IsSuccessStatusCode();
            this.Location = location;
            if (string.IsNullOrWhiteSpace(location) &&
                (statusCode == HttpStatusCode.Created || statusCode == HttpStatusCode.Moved))
            {
                this.Location = "/"; // not accessible
            }
            else
            {
                this.Location = location;
            }

            if (string.IsNullOrWhiteSpace(message) && exceptionObject != null
                                                   && !string.IsNullOrWhiteSpace(exceptionObject.Message) &&
                                                   (statusCode == HttpStatusCode.BadRequest ||
                                                    statusCode == HttpStatusCode.Conflict))
            {
                this.Message = exceptionObject.Message;
            }
            else
            {
                this.Message = message;
            }
        }

        /// <summary>
        ///     Convenience property to check the http status code result
        /// </summary>
        public bool IsSuccessStatusCode { get; }

        /// <summary>
        ///     Any exception thrown while requesting the result
        /// </summary>
        public Exception ExceptionObject { get; }

        /// <summary>
        ///     The data returned from the api request
        /// </summary>
        public T Data { get; }

        /// <summary>
        ///     The http status code from the http request
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        ///     If the request created an entity then the hypermedia location of the created
        ///     entity should be set here.
        ///     example: <code>http://localhost/dog/1</code>
        /// </summary>
        public string Location { get; }

        /// <summary>
        ///     Additional meta data as needed by the result. This should be used to set
        ///     messages for the client when more than one situation can result in the same
        ///     status code, but the client might want more information to correct the issue.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// </summary>
        /// <param name="apiResult"></param>
        public static implicit operator ApiResult(ApiResult<T> apiResult)
        {
            return new ApiResult(
                apiResult.StatusCode,
                apiResult.Data,
                apiResult.IsSuccessStatusCode,
                apiResult.Location,
                apiResult.Message,
                apiResult.ExceptionObject);
        }
    }
}