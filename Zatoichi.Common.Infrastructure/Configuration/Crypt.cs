namespace Zatoichi.Common.Infrastructure.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Crypt
    {
        public bool CompressText { get; set; }
        public int Iterations { get; set; }
        public string Key { get; set; }
        public string Password { get; set; }
    }
}