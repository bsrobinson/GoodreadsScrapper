namespace GoodreadsScrapper.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public List<BookInSeries> Books {get;set;}

        public Series(int id)
        {
            Id = id;
            Books = new();
        }
    }
}