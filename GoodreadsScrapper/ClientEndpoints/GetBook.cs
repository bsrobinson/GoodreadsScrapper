using System;
using System.Text.RegularExpressions;
using GoodreadsScrapper.Extensions;
using GoodreadsScrapper.Models;
using HtmlAgilityPack;

namespace GoodreadsScrapper
{
	public partial class GoodreadsClient
    {
        public async Task<Book?> GetBookAsync(int id, bool includeSeries = false)
        {
            return await GetBookAsync($"https://www.goodreads.com/book/show/{id}", includeSeries);
        }

		public async Task<Book?> GetBookAsync(string url, bool includeSeries = false)
		{
            HtmlWeb web = new HtmlWeb();
            HtmlNode doc = (await web.LoadFromWebAsync(url)).DocumentNode;

            HtmlNode? author = doc.SelectNodes(".//*[@itemprop='author']")?.FirstOrDefault();
            string? authorName = author?.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
            string? authorUrl = author?.SelectNodes(".//*[@itemprop='url']")?.FirstOrDefault()?.GetAttributeValue("href", null)?.CompleteUrl();
            if (author != null)
            {
                author.Remove();
            }

            int? id = doc.SelectNodes(".//*[@id='book_id']")?.FirstOrDefault()?.GetAttributeValue("value", (int?)null);
            string? name = doc.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
            double.TryParse(doc.SelectNodes(".//*[@itemprop='ratingValue']")?.FirstOrDefault()?.CleanText(), out double rating);

            Match publishYearMatch = new Regex("first published.{1,25}?(\\d{4})").Match(doc.InnerHtml);
            if (!publishYearMatch.Success)
            {
                publishYearMatch = new Regex("Published.{1,25}?(\\d{4})").Match(doc.InnerHtml);
            }

            HtmlNode? seriesNode = doc.SelectNodes(".//*[@class='seriesList']")?.FirstOrDefault();
            HtmlNode? seriesLink = seriesNode?.SelectNodes(".//a[contains(@href, '/series/')]")?.FirstOrDefault();
            string? seriesUrl = seriesLink?.GetAttributeValue("href", null)?.CompleteUrl();

            Series? series = null;
            if (seriesUrl != null && includeSeries)
            {
                series = await GetBookSeriesAsync(seriesUrl, id);
            }

            if (id != null && name != null)
            {
                return new((int)id, name)
                {
                    Image = doc.SelectNodes(".//img[@id='coverImage']")?.FirstOrDefault()?.GetAttributeValue("src", null)?.CompleteUrl(),
                    Url = url,
                    Author = authorName == null ? null : new(authorName) { Url = authorUrl },
                    PublishedYear = publishYearMatch.Success ? int.Parse(publishYearMatch.Groups[1].Value) : null,
                    Rating = rating,
                    RatingCount = doc.SelectNodes(".//*[@itemprop='ratingCount']")?.FirstOrDefault()?.GetAttributeValue("content", (int?)null),
                    ReviewCount = doc.SelectNodes(".//*[@itemprop='reviewCount']")?.FirstOrDefault()?.GetAttributeValue("content", (int?)null),
                    Series = series
                };
            }

            return null;
        }
    }
}

