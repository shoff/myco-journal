namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public static class EnumerableExtensions
    {
        private static Random rng = new Random();

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> function)
        {
            foreach (var item in enumerable)
            {
                function(item);
            }
        }

        public static IEnumerable<TR> Map<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> function)
        {
            foreach (var item in enumerable)
            {
                yield return function(item);
            }
        }

        public static string ToDescription(this Enum enumeration)
        {
            var customAttributes = enumeration.GetType().GetMember(enumeration.ToString())[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttributes.Length == 0 ? null : ((DescriptionAttribute) customAttributes[0]).Description;
        }

        public static void Shuffle<T>(this T[] array)
        {
            rng = new Random();
            var num = array.Length;
            while (num > 1)
            {
                var num2 = rng.Next(num);
                num--;
                var val = array[num];
                array[num] = array[num2];
                array[num2] = val;
            }
        }
    }
}