namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Mvc;

    [ExcludeFromCodeCoverage]
    public class AspnetCoreOptions
    {
        [JsonIgnore]
        public Action<MvcOptions> MvcOptions { get; set; } = options => { };

        public string ResourcesPath { get; set; } = "Resources";
        public ICollection<string> SupportedCultures { get; set; } = new List<string>();
        public bool UseCors { get; set; }
        public CorsOptions Cors { get; set; } = new CorsOptions();
        public ClientSecurityOptions ClientSecurity { get; set; } = new ClientSecurityOptions();
        public JwtOptions Jwt { get; set; } = new JwtOptions();
        public bool UseIIS { get; set; }
        public string UseUrls { get; set; }
        public bool CaptureStartupErrors { get; set; }
        public bool UseForwardedHeaders { get; set; }
        public bool UseAngularFolder { get; set; }
        public bool UseStaticFiles { get; set; }
        public bool UseCookiePolicy { get; set; }

        /// <summary>No SameSite field will be set, the client should follow its default cookie policy.</summary>
        /// Unspecified = -1, // 0xFFFFFFFF
        /// <summary>Indicates the client should disable same-site restrictions.</summary>
        ///None = 0,
        /// <summary>Indicates the client should send the cookie with "same-site" requests, and with "cross-site" top-level navigations.</summary>
        ///Lax = 1,
        /// <summary>Indicates the client should only send the cookie with "same-site" requests.</summary>
        /// Strict = 2,
        public int SafeSiteMode { get; set; } = 1;
    }}