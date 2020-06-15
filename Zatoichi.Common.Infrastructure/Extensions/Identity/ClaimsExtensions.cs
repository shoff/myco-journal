namespace Zatoichi.Common.Infrastructure.Extensions.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsExtensions
    {
        public const string USERNAME_CLAIM = "username";

        public static string UserName(this IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();
            return enumerable.FirstOrDefault(c => c.Type == "username")?.Value;
        }
    }
}