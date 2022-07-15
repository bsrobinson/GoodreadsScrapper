using System.Web;
using HtmlAgilityPack;

namespace GoodreadsScrapper.Extensions
{
    internal static class HtmlNodeExtensions
    {
        public static string? CleanText(this HtmlNode? node) =>
            node == null ? null : HttpUtility.HtmlDecode(node.InnerText.Trim());
    }
}
