namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class JwtOptions
    {
        public string Authority { get; set; }

        public string Audience { get; set; }
    }
}