using System.Text.RegularExpressions;
using GoodreadsScrapper.Extensions;
using GoodreadsScrapper.Models;
using HtmlAgilityPack;

namespace GoodreadsScrapper
{
    public partial class GoodreadsClient
    {
        public async Task<Series?> GetBookSeriesAsync(int id, int? forBookId = null)
        {
            return await GetBookSeriesAsync($"https://www.goodreads.com/series/{id}", forBookId);
        }

        public async Task<Series?> GetBookSeriesAsync(string url, int? forBookId = null)
        {
            List<BookInSeries> searchResults = new();

            HtmlWeb web = new HtmlWeb();
            HtmlNode doc = (await web.LoadFromWebAsync(url)).DocumentNode;

            HtmlNodeCollection results = doc.SelectNodes("//*[@itemtype='http://schema.org/Book']");
            if (results != null)
            {
                foreach (HtmlNode result in results)
                {
                    double? seriesPosition = null;
                    if (result.PreviousSibling != null)
                    {
                        Match seriesPositionMatch = new Regex("Book ([0-9.]+)$", RegexOptions.IgnoreCase).Match(result.PreviousSibling.CleanText() ?? "");
                        if (seriesPositionMatch.Success)
                        {
                            seriesPosition = double.Parse(seriesPositionMatch.Groups[1].Value);
                        }
                    }

                    if (seriesPosition != null)
                    {
                        HtmlNode? author = result.SelectNodes(".//*[@itemprop='author']")?.FirstOrDefault();
                        string? authorName = author?.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
                        string? authorUrl = author?.SelectNodes(".//*[@itemprop='url']")?.FirstOrDefault()?.GetAttributeValue("href", null)?.CompleteUrl();
                        if (author != null)
                        {
                            author.Remove();
                        }

                        string? name = result.SelectNodes(".//*[@itemprop='name']")?.FirstOrDefault()?.CleanText();
                        string? image = result.SelectNodes(".//*[@itemprop='image']")?.FirstOrDefault()?.GetAttributeValue("src", null)?.CompleteUrl();
                        string? bookUrl = result.SelectNodes(".//*[@itemprop='url']")?.FirstOrDefault()?.GetAttributeValue("href", null)?.CompleteUrl();
                        bool idParsed = int.TryParse(bookUrl?.LastPathComponent()?.IdFromSlug() ?? "", out int id);
                        Match publishYearMatch = new Regex("published.*?(\\d{4})", RegexOptions.Singleline).Match(result.InnerText);

                        if (idParsed && name != null)
                        {
                            searchResults.Add(new((int)id, name)
                            {
                                Image = image,
                                Url = bookUrl,
                                Author = authorName == null ? null : new(authorName) { Url = authorUrl },
                                PublishedYear = publishYearMatch.Success ? int.Parse(publishYearMatch.Groups[1].Value) : null,
                                SeriesPosition = (double)seriesPosition,
                            });
                        }
                    }
                }
            }

            if (forBookId == null || searchResults.FirstOrDefault(b => b.Id == forBookId) != null)
            {
                int id = int.Parse(url.LastPathComponent()?.IdFromSlug() ?? "0");

                string? titleText = doc.SelectNodes(".//title")?.FirstOrDefault()?.CleanText();
                Match nameMatch = new Regex("(.*?) Series").Match(titleText ?? "");

                return new Series(id)
                {
                    Name = nameMatch.Groups[1].Value ?? null,
                    Url = url,
                    Books = searchResults
                };
            }
            return null;
        }
    }
}

