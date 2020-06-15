namespace Zatoichi.Common.Infrastructure
{
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage] // constants don't need coverage
    public static class Constants
    {
        public const string LOCAL_ENVIRONMENT = "Local";
        public const string ASPNET_CORE_OPTIONS = "AspnetCoreOptions";
        public static readonly ImmutableHashSet<int> TransientSqlErrorNumbers = ImmutableHashSet.Create(-1, -2, 2, 53);

        public static readonly ImmutableHashSet<int> TransientHttpStatusCodes =
            ImmutableHashSet.Create(408, 416, 418, 420, 423, 426, 429, 500, 502, 503, 504, 598);
    }
}