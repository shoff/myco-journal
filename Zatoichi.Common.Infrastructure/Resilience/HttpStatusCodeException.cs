namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Runtime.Serialization;

    [Serializable]
    public class HttpStatusCodeException : ApplicationException
    {
        public HttpStatusCodeException(HttpStatusCode statusCode)
            : this(statusCode, $"An HttpRequest returned a status code of {statusCode}")
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, Exception exception)
            : this(statusCode, $"An HttpRequest returned a status code of {statusCode}", exception)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message)
            : this(statusCode, message, null)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        [ExcludeFromCodeCoverage]
        protected HttpStatusCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HttpStatusCode StatusCode { get; protected set; }

        public bool IsTransient => TransientErrors.TransientHttpStatusCodes.Contains((int) this.StatusCode);
    }
}