namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ChaosMonkey.Guards;

    public static class DirectoryExtensions
    {
        public static IEnumerable<string> GetFiles(string path, string searchPattern, IEnumerable<string> exclude,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            var enumerable = exclude as string[] ?? exclude.ToArray();
            Guard.IsNotNullOrWhitespace(path, nameof(path));
            Guard.IsNotNullOrWhitespace(searchPattern, nameof(searchPattern));
            var enumerable2 = new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption);
            foreach (var filename in enumerable2)
            {
                if (!enumerable.Any(x =>
                    x == filename.Name.ToLowerInvariant() || x.StartsWith("*") && x.Contains(filename.Extension)))
                {
                    yield return filename.Name;
                }
            }
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            Guard.IsNotNullOrWhitespace(path, nameof(path));
            Guard.IsNotNullOrWhitespace(searchPattern, nameof(searchPattern));
            var array = searchPattern.Split('|');
            var list = new List<string>();
            var array2 = array;
            foreach (var searchPattern2 in array2)
            {
                list.AddRange(Directory.GetFiles(path, searchPattern2, searchOption));
            }

            return list.ToArray();
        }
    }
}