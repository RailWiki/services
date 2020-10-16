using System.Text.RegularExpressions;

namespace RailWiki.Shared.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Cleans a string that can be used as a URL slug
        /// </summary>
        /// <param name="input">String to clean</param>
        /// <returns>Cleaned string</returns>
        public static string Slugify(this string input)
        {
            var regex = new Regex("[^a-z0-9\\-_]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            input = input.Replace(" ", "-");
            var cleaned = regex.Replace(input, "").ToLower();

            while (cleaned.Contains("--"))
            {
                cleaned = cleaned.Replace("--", "-");
            }

            return cleaned;
        }
    }
}
