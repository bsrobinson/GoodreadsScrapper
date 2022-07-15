using System;
using System.Text.RegularExpressions;
using GoodreadsScrapper.Extensions;
using GoodreadsScrapper.Models;
using HtmlAgilityPack;

namespace GoodreadsScrapper
{
	public partial class GoodreadsClient
	{
		public async Task<List<SearchBook>> SearchBooksAsync(string query)
		{
            string _url = $"https://www.goodreads.com/search?q={query}&search%5Bsource%5D=goodreads&search_type=books&tab=books";

            List<SearchBook> searchResults = new();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = await web.LoadFromWebAsync(_url);

            HtmlNodeCollection results = document.DocumentNode.SelectNodes("//*[@itemtype='http://schema.org/Book']");
            if (results != null)
            {
                foreach (HtmlNode result in results)
                {
                    HtmlNode? author = result.SelectNodes(".//*[@itemprop='author']")?.FirstOrDefault();
                    string? authorName = author?.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
                    string? authorUrl = author?.SelectNodes(".//*[@itemprop='url']")?.FirstOrDefault()?.GetAttributeValue("href", null)?.CompleteUrl();
                    if (author != null)
                    {
                        author.Remove();
                    }

                    int? id = result.SelectNodes(".//*[@id='book_id']")?.FirstOrDefault()?.GetAttributeValue("value", (int?)null);
                    string? name = result.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
                    string? image = result.SelectNodes(".//*[@itemprop='image']")?.FirstOrDefault()?.GetAttributeValue("src", null)?.CompleteUrl();
                    string? url = result.SelectNodes(".//*[@itemprop='url']")?.FirstOrDefault()?.GetAttributeValue("href", null)?.CompleteUrl();
                    Match publishYearMatch = new Regex("published.*?(\\d{4})", RegexOptions.Singleline).Match(result.InnerText);

                    if (id != null && name != null)
                    {
                        searchResults.Add(new((int)id, name)
                        {
                            Image = image,
                            Url = url,
                            Author = authorName == null ? null : new(authorName) { Url = authorUrl },
                            PublishedYear = publishYearMatch.Success ? int.Parse(publishYearMatch.Groups[1].Value) : null
                        });
                    }
                }
            }

            return searchResults;
        }
    }
}

