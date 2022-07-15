# GoodreadsScrapper

I've arrived at Goodreads wanting to use some of their data only to see the API has been shut down, so I wrote this scraper to collect the information I needed.

For my purposes I only needed a small number of endpoints, and only public facing information, so this code is pretty simple, but I'm happy for people to expand on the data this can fetch.

## Installation

Available on Nuget as `GoodreadsScrapper`

## Usage

```csharp
using GoodreadsScrapper;
using GoodreadsScrapper.Models;

GoodreadsClient client = new();
List<SearchBook> searchResults = client.SearchBooksAsync("potter");

Book bookDetails = client.GetBookAsync(searchResults.First().Url);
```

## Endpoints

### SearchBooksAsync

Passes your query to the book search page and returns a list of SearchBook objects containing basic details of each book.  If no results will return an empty list.

```csharp
Task<List<SearchBook>> SearchBooksAsync(string query)
```

### GetBookAsync

Get full book details from the book page, optionally with the series information included (using the `GetBookSeriesAsync` endpoint).  You can pass either the book id or the full url of the book page, both work but Url is recommended if you have it.  If the book page isn't found or is unavailable will return null.

```csharp
Task<Book?> GetBookAsync(string url, [bool includeSeries = false])
```

```csharp
Task<Book?> GetBookAsync(int id, [bool includeSeries = false])
```

### GetBookSeriesAsync

Gets all books in a series.  You can pass either the series id or the full url of the series page, both work but Url is recommended if you have it.  Returns a Series object including the list of books, or null if the page isn't found or available.

Books series pages often list many titles, not all of which are formal parts of the series and can include box-set publications.  This endpoint will only return formal single editions, i.e. ones labeled with a Book Number on the series page.

Sometimes a book apears to be part of a series (on the book page), but the book isn't actually a formal member of the list.  You can optionally pass the book id to ensure the series includes the book you're looking at, if not returns null.

```csharp
Task<Series?> GetBookSeriesAsync(string url, [int? forBookId = null])
```

```csharp
Task<Series?> GetBookSeriesAsync(int id, [int? forBookId = null])
```

## Models

Have a look in the Models folder for the information available from these endpoints, hopfully they are stright-forward.