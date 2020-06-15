namespace Zatoichi.Common.Infrastructure.Extensions.Identity
{
    using System.Security.Claims;
    using Configuration;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class JwtSecurityExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication("Bearer").AddJwtBearer(delegate(JwtBearerOptions options)
            {
                options.Audience = jwtOptions.Audience;
                options.Authority = jwtOptions.Authority;
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    RequireExpirationTime = true,
                    RequireSignedTokens = false,
                    ValidateAudience = true,
                    ValidAudiences = new[] {jwtOptions.Audience},
                    ValidateIssuer = true
                };
            });
            return services;
        }
    }
}