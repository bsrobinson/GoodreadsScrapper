using System.Text.Json;
using GoodreadsScrapper;

GoodreadsClient client = new();


string searchString = "potter";

Console.WriteLine($"Searching books for {searchString}");
var books = await client.SearchBooksAsync(searchString);

Console.WriteLine($"Found {books.Count} books:");
books.ForEach(book => Console.WriteLine($"{book.Id} - {book.Name}"));


var bookToGet = books.ElementAt(new Random().Next(books.Count));

Console.WriteLine($"Getting book {bookToGet.Id} {bookToGet.Name}");
var book = await client.GetBookAsync(bookToGet.Id, includeSeries: true);

Console.WriteLine(JsonSerializer.Serialize(book));