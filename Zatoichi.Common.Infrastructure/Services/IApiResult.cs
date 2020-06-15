namespace Zatoichi.Common.Infrastructure.Services
{
    using System;
    using System.Net;

    public interface IApiResult
    {
        public bool IsSuccessStatusCode { get; }

        /// <summary>
        ///     Any exception thrown while requesting the result
        /// </summary>
        Exception ExceptionObject { get; }

        /// <summary>
        ///     The data returned from the api request
        /// </summary>
        object Data { get; }

        /// <summary>
        ///     The http status code from the http request
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        ///     If the request created an entity then the hypermedia location of the created
        ///     entity should be set here.
        ///     example: <code>http://localhost/dog/1</code>
        /// </summary>
        string Location { get; }

        /// <summary>
        ///     Additional meta data as needed by the result. This should be used to set
        ///     messages for the client when more than one situation can result in the same
        ///     status code, but the client might want more information to correct the issue.
        /// </summary>
        string Message { get; }
    }

    public interface IApiResult<out T>
    {
        /// <summary>
        ///     Convenience property to check the http status code result
        /// </summary>
        bool IsSuccessStatusCode { get; }

        /// <summary>
        ///     Any exception thrown while requesting the result
        /// </summary>
        Exception ExceptionObject { get; }

        /// <summary>
        ///     Additional meta data as needed by the result. This should be used to set
        ///     messages for the client when more than one situation can result in the same
        ///     status code, but the client might want more information to correct the issue.
        /// </summary>
        string Message { get; }

        /// <summary>
        ///     The data returned from the api request
        /// </summary>
        T Data { get; }

        /// <summary>
        ///     The http status code from the http request
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        ///     If the request created an entity then the hypermedia location of the created
        ///     entity should be set here.
        ///     example: <code>http://localhost/dog/1</code>
        /// </summary>
        string Location { get; }
    }
}