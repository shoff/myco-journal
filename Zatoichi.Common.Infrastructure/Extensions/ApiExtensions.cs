namespace Zatoichi.Common.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Services;

    public static class ApiExtensions
    {
        internal const string JSON_TEXT = "application/json";
        private static readonly Dictionary<HttpStatusCode, Func<ApiResult, IActionResult>> strategyDictionary;

        static ApiExtensions()
        {
            strategyDictionary = new Dictionary<HttpStatusCode, Func<ApiResult, IActionResult>>();
            strategyDictionary.Add(HttpStatusCode.OK, Ok);
            strategyDictionary.Add(HttpStatusCode.BadRequest, BadRequest);
            strategyDictionary.Add(HttpStatusCode.NotFound, NotFound);
            strategyDictionary.Add(HttpStatusCode.InternalServerError, InternalServerError);
            strategyDictionary.Add(HttpStatusCode.Accepted, Accepted);
            strategyDictionary.Add(HttpStatusCode.Created, Created);
            strategyDictionary.Add(HttpStatusCode.Conflict, BadRequest);
            strategyDictionary.Add(HttpStatusCode.Forbidden, Forbidden);
            strategyDictionary.Add(HttpStatusCode.Unauthorized, Unauthorized);
            strategyDictionary.Add(HttpStatusCode.BadGateway, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.Found, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.AlreadyReported, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.MultipleChoices, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.EarlyHints, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.Continue, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.ExpectationFailed, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.FailedDependency, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.GatewayTimeout, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.HttpVersionNotSupported, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.NotModified, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.InsufficientStorage, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.Gone, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.VariantAlsoNegotiates, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.UseProxy, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.UpgradeRequired, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.UnsupportedMediaType, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.Unused, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.UnavailableForLegalReasons, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.TooManyRequests, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.IMUsed, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.UnprocessableEntity, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.SeeOther, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.MultiStatus, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.Locked, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.LoopDetected, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.NetworkAuthenticationRequired, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.NonAuthoritativeInformation, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.NoContent, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.PermanentRedirect, StatusCodeResult);
            strategyDictionary.Add(HttpStatusCode.TemporaryRedirect, StatusCodeResult);
        }

        public static IActionResult ToActionResult<T, TU>(this ApiResult<T> api, TU data) where T : class
            where TU : class
        {
            return new ApiResult<TU>(api.StatusCode, data, api.IsSuccessStatusCode, api.Location, api.Message,
                api.ExceptionObject).ToActionResult();
        }

        public static IActionResult ToActionResult<T>(this ApiResult<T> api)
        {
            if (api == null)
            {
                return new InternalServerErrorResult(
                    new UnexpectedStateException("An unknown error has occured on the server."))
                {
                    StatusCode = 500
                };
            }

            return strategyDictionary[api.StatusCode](api);
        }

        internal static IActionResult Ok(ApiResult api)
        {
            return new OkObjectResult(api.Data)
            {
                ContentTypes = new MediaTypeCollection
                {
                    JSON_TEXT
                },
                DeclaredType = api.Data.GetType(),
                StatusCode = (int) api.StatusCode,
                Value = api.Data
            };
        }

        internal static IActionResult StatusCodeResult(ApiResult api)
        {
            return new StatusCodeResult((int) api.StatusCode);
        }

        internal static IActionResult Unauthorized(ApiResult api)
        {
            return new UnauthorizedResult();
        }

        internal static IActionResult Forbidden(ApiResult api)
        {
            return new ForbidResult();
        }

        internal static IActionResult Created(ApiResult api)
        {
            return new CreatedResult(api.Location, api)
            {
                ContentTypes = new MediaTypeCollection
                {
                    JSON_TEXT
                },
                DeclaredType = api.Data.GetType(),
                StatusCode = (int) api.StatusCode,
                Value = string.IsNullOrWhiteSpace(api.Message) ? api.Data : api.Message,
                Location = api.Location
            };
        }

        internal static IActionResult Accepted(ApiResult api)
        {
            return new AcceptedResult("", api)
            {
                ContentTypes = new MediaTypeCollection
                {
                    JSON_TEXT
                },
                DeclaredType = api.Data.GetType(),
                StatusCode = (int) api.StatusCode,
                Value = string.IsNullOrWhiteSpace(api.Message) ? api.Data : api.Message
            };
        }

        internal static IActionResult BadRequest(ApiResult api)
        {
            return new BadRequestObjectResult(api.ExceptionObject)
            {
                Value = string.IsNullOrWhiteSpace(api.Message) ? api.Data : api.Message
            };
        }

        internal static IActionResult NotFound(ApiResult api)
        {
            return new NotFoundObjectResult(api)
            {
                ContentTypes = new MediaTypeCollection
                {
                    JSON_TEXT
                },
                DeclaredType = api.Data.GetType(),
                StatusCode = 404,
                Value = api.Message
            };
        }

        internal static IActionResult InternalServerError(ApiResult api)
        {
            return new InternalServerErrorResult(api.ExceptionObject)
            {
                StatusCode = 500,
                Value = api.Message
            };
        }
    }
}