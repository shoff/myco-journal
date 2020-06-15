namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System;

    public static class FloatExtensions
    {
        public static bool NearlyEquals(this double? value1, double? value2, double unimportantDifference = 0.0001)
        {
            if (value1 != value2)
            {
                if (!value1.HasValue || !value2.HasValue)
                {
                    return false;
                }

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }
    }
}