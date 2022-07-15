using System.Text.RegularExpressions;

namespace GoodreadsScrapper.Extensions
{
    internal static class StringExtensions
    {
        public static string CompleteUrl(this string url) =>
            url.StartsWith("/") ? url = $"https://www.goodreads.com{url}" : url;

        public static string? LastPathComponent(this string url) =>
            url == null ? null : url.Split("/").LastOrDefault(c => !string.IsNullOrEmpty(c));

        public static string? IdFromSlug(this string slug)
        {
            if (slug != null)
            {
                Match match = new Regex("(\\d+)[^\\d]").Match(slug);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            return null;
        }
    }
}
