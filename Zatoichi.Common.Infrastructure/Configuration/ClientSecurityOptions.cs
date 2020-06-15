namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ClientSecurityOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();
    }
}