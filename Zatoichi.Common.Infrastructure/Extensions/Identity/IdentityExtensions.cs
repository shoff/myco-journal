namespace Zatoichi.Common.Infrastructure.Extensions.Identity
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Microsoft.AspNetCore.Identity;

    public static class IdentityExtensions
    {
        public static string ToMessage(this IEnumerable<IdentityError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            return string.Join(" ,", errors.Map(e => $"Code: {e.Code} Message: {e.Description}"));
        }
    }
}