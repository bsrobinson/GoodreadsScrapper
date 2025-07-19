using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using GoodreadsScrapper.Extensions;
using GoodreadsScrapper.Models;
using GoodreadsScrapper.Models.JsonLd;
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

            var ldJsonScript = doc.SelectSingleNode("//script[@type='application/ld+json']");
            JsonLdBook? bookData = JsonSerializer.Deserialize<JsonLdBook>(ldJsonScript.InnerHtml);

            int id = 0;
            Match idMatch = new Regex(@"""book_id"":""(\d*?)""").Match(doc.InnerHtml);
            bool idParsed = idMatch.Success ? int.TryParse(idMatch.Groups[1].Value, out id) : false;

            int year = 0;
            Match publishYearMatch = new Regex("First published.{1,25}?(\\d{4})").Match(doc.InnerHtml);
            if (!publishYearMatch.Success)
            {
                publishYearMatch = new Regex("Published.{1,25}?(\\d{4})").Match(doc.InnerHtml);
            }
            bool yearParsed = publishYearMatch.Success ? int.TryParse(publishYearMatch.Groups[1].Value, out year) : false;

            Series? series = null;
            if (includeSeries)
            {
                Match seriesMatch = new Regex(@"goodreads.com/series/(\d*)").Match(doc.InnerHtml);
                if (seriesMatch.Success && int.TryParse(seriesMatch.Groups[1].Value, out int seriesId))
                {
                    series = await GetBookSeriesAsync(seriesId, id);
                }
            }

            if (bookData != null && idParsed)
            {
                return new(id, bookData, url, yearParsed ? year : null, series);
            }

            return null;
        }
    }
}

