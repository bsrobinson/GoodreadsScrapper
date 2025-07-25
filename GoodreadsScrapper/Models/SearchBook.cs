﻿using GoodreadsScrapper.Models.JsonLd;

namespace GoodreadsScrapper.Models
{
    public class SearchBook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public Author? Author { get; set; }
        public int? PublishedYear { get; set; }

        public SearchBook(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public SearchBook(int id, JsonLdBook book, string url, int? publishedYear)
        {
            Id = id;
            Name = book.Name;
            Image = book.Image;
            Url = url;
            Author = book.Author.Count > 0 ? new(book.Author[0]) : null;
            PublishedYear = publishedYear;
        }
    }
}