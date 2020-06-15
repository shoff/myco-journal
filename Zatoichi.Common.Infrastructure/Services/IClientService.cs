namespace Zatoichi.Common.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IClientService
    {
        /// <summary>
        ///     Executes a POST request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        Task<ApiResult<T>> PostAsync<TU, T>(string url, TU postModel);

        /// <summary>
        ///     Executes a PATCH request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        Task<ApiResult<T>> PatchAsync<TU, T>(string url, TU postModel);

        /// <summary>
        ///     Executes a PUT request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        Task<ApiResult<T>> PutAsync<TU, T>(string url, TU postModel);

        /// <summary>
        ///     Executes a POST request putting the passed in content into the
        ///     request as a application/x-www-form-urlencoded key value collection.
        ///     Adds and removes the required headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<ApiResult<T>> PostFormContentAsync<T>(string url, HttpContent content);

        /// <summary>
        ///     Executes a POST request putting the passed in data into the
        ///     request as a application/x-www-form-urlencoded key value collection.
        ///     Adds and removes the required headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ApiResult<T>> PostFormContentAsync<T>(string url, ICollection<KeyValuePair<string, string>> data);

        /// <summary>
        ///     Issues an asynchronous Http Get request to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns>A <c>ApiResult</c></returns>
        /// <remarks>Ob</remarks>
        Task<ApiResult<T>> GetJsonAsync<T>(string url);

        /// <summary>
        ///     Issues an asynchronous Http Get request to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters">
        ///     A params collection of <c>System.Collections.Generic.KeyValuePair&lt;string,string&gt;</c>
        ///     that represent the query string keys and values
        /// </param>
        /// <returns>A <c>ApiResult</c></returns>
        /// <remarks>Object property names must match</remarks>
        Task<ApiResult<T>> GetJsonAsync<T>(string url, IDictionary<string, string> parameters);

        /// <summary>
        ///     Issues an asynchronous Http Get request to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="url">The Url that is requested via HTTP Get</param>
        /// <param name="requestObject">The object to be converted to a Url query string</param>
        /// <returns>A <c>ApiResult</c></returns>
        /// <remarks>Object property names must match</remarks>
        Task<ApiResult<T>> GetJsonAsync<TU, T>(string url, TU requestObject);

        /// <summary>
        ///     Adds a bearer auth token header to any requests in this http client instance,
        ///     or replaces an expired one through the passed in <seealso cref="IClientTokenProvider" />
        /// </summary>
        /// <param name="tokenProvider"></param>
        /// <returns></returns>
        Task AddBearerTokenAsync(IClientTokenProvider tokenProvider);
    }
}