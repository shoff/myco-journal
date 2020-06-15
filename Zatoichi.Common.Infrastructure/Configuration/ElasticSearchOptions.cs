namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ElasticSearchOptions
    {
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
}