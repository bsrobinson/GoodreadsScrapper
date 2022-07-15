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
    }
}