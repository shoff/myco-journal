namespace Zatoichi.Common.Infrastructure.Extensions
{
    using System.Net;

    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode httpStatusCode)
        {
            return httpStatusCode < HttpStatusCode.BadRequest;
        }
    }
}