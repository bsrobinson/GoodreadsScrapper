using GoodreadsScrapper.Models.JsonLd;

namespace GoodreadsScrapper.Models
{
    public class Book : SearchBook
    {
        public double? Rating { get; set; }
        public int? RatingCount { get; set; }
        public int? ReviewCount { get; set; }
        public Series? Series { get; set; }

        public Book(int id, string name) : base(id, name)
        {
        }

        public Book(int id, JsonLdBook book, string url, int? publishedYear, Series? series) : base(id, book, url, publishedYear)
        {
            if (book.AggregateRating != null)
            {
                Rating = book.AggregateRating.RatingValue;
                RatingCount = book.AggregateRating.RatingCount;
                ReviewCount = book.AggregateRating.ReviewCount;
                Series = series;
            }
        }
    }
}