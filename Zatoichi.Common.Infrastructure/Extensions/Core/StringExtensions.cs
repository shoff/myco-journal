namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using ChaosMonkey.Guards;

    public static class StringExtensions
    {
        public static string RemoveTrailingComma(this string value)
        {
            Guard.IsNotNullOrWhitespace(value, nameof(value));
            if (value.EndsWith(','))
            {
                return value.Substring(0, value.Length - 1);
            }

            return value;
        }

        public static bool EqualsIgnoreCase(this string value, string otherValue)
        {
            return string.Compare(value, otherValue, true, CultureInfo.InvariantCulture) == 0;
        }

        public static string Sha256Hash(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var array = SHA256.Create().ComputeHash(Encoding.Default.GetBytes(value));
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < array.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", array[i]);
            }

            return stringBuilder.ToString();
        }
    }
}