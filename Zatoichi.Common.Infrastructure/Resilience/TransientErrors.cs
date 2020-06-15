namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class TransientErrors
    {
        public static readonly ImmutableHashSet<int> TransientSqlErrorNumbers = ImmutableHashSet.Create(-1, -2, 2, 53);

        public static readonly ImmutableHashSet<int> TransientHttpStatusCodes =
            ImmutableHashSet.Create(408, 416, 418, 420, 423, 426, 429, 500, 502, 503, 504, 598);
    }
}