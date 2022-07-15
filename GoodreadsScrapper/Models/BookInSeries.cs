namespace GoodreadsScrapper.Models
{
    public class BookInSeries : SearchBook
    {
        public double SeriesPosition { get; set; }
        
        public BookInSeries(int id, string name) : base(id, name)
        {
        }
    }
}