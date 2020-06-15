namespace Zatoichi.Common.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Resilience;

    [ExcludeFromCodeCoverage]
    public abstract class ClientService : IClientService
    {
        protected readonly HttpClient httpClient;
        protected readonly ILogger<ClientService> logger;

        protected ClientService(
            HttpClient httpClient,
            ILogger<ClientService> logger)
        {
            this.logger = logger ?? throw new TypeInitializationException(typeof(ClientService).FullName,
                new ArgumentNullException(nameof(logger)));
            this.httpClient = httpClient ?? throw new TypeInitializationException(typeof(ClientService).FullName,
                new ArgumentNullException(nameof(this.httpClient)));
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///     Executes a POST request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PostAsync<TU, T>(string url, TU postModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(postModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await this.httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);

                return new ApiResult<T>(HttpStatusCode.InternalServerError, default, false, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Executes a PATCH request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PatchAsync<TU, T>(string url, TU postModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(postModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) {Content = content};
                var response = await this.httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Executes a PUT request to the given URL with the
        ///     passed in model as a json object in the request body.
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PutAsync<TU, T>(string url, TU postModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(postModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PUT"), url) {Content = content};
                var response = await this.httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);

                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Executes a POST request putting the passed in content into the
        ///     request as a application/x-www-form-urlencoded key value collection.
        ///     Adds and removes the required headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PostFormContentAsync<T>(string url, HttpContent content)
        {
            try
            {
                var response = await this.httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);

                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Executes a POST request putting the passed in data into the
        ///     request as a application/x-www-form-urlencoded key value collection.
        ///     Adds and removes the required headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ApiResult<T>> PostFormContentAsync<T>(string url,
            ICollection<KeyValuePair<string, string>> data)
        {
            var content = new FormUrlEncodedContent(data);
            return await PostFormContentAsync<T>(url, content);
        }

        /// <summary>
        ///     Issues an asynchronous Http Get request to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns>A <c>ApiResult</c></returns>
        /// <remarks>Ob</remarks>
        public async Task<ApiResult<T>> GetJsonAsync<T>(string url)
        {
            try
            {
                var response = await this.httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

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
        public async Task<ApiResult<T>> GetJsonAsync<T>(string url, IDictionary<string, string> parameters)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"{url}{KeyValuesToQuery(parameters)}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Issues an asynchronous Http Get request to the given url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="url">The Url that is requested via HTTP Get</param>
        /// <param name="requestObject">The object to be converted to a Url query string</param>
        /// <returns>A <c>ApiResult</c></returns>
        /// <remarks>Object property names must match</remarks>
        public async Task<ApiResult<T>> GetJsonAsync<TU, T>(string url, TU requestObject)
        {
            try
            {
                var urlList = new List<string>();
                foreach (PropertyDescriptor property in
                    TypeDescriptor.GetProperties(requestObject))
                {
                    urlList.Add(property.Name + "=" + property.GetValue(requestObject));
                }

                var response = await this.httpClient.GetAsync($"{url}?{string.Join("&", urlList)}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<T>(result);
                    return new ApiResult<T>(response.StatusCode, t, true);
                }

                return new ApiResult<T>(response.StatusCode, default, false,
                    exceptionObject: new HttpStatusCodeException(response.StatusCode));
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                return new ApiResult<T>(HttpStatusCode.InternalServerError,
                    default, false, message: e.Message, exceptionObject: e);
            }
        }

        /// <summary>
        ///     Adds a bearer auth token header to any requests in this http client instance,
        ///     or replaces an expired one through the passed in <seealso cref="IClientTokenProvider" />
        /// </summary>
        /// <param name="tokenProvider"></param>
        /// <returns></returns>
        public async Task AddBearerTokenAsync(IClientTokenProvider tokenProvider)
        {
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await tokenProvider.GetTokenAsync());
        }

        private static string KeyValuesToQuery(IDictionary<string, string> parameters)
        {
            var querySegments =
                $"?{string.Join("&", parameters.Select(query => WebUtility.UrlEncode(query.Key) + "=" + WebUtility.UrlEncode(query.Value)))}";
            return querySegments;
        }
    }
}