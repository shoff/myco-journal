namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CorsOptions
    {
        public ICollection<string> Headers { get; set; } = new List<string>();
        public ICollection<string> Methods { get; set; } = new List<string>();
        public ICollection<string> Origins { get; set; } = new List<string>();
    }
}