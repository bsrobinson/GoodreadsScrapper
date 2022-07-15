﻿namespace GoodreadsScrapper.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string? Url { get; set; }

        public Author(string name)
        {
            Name = name;
        }
    }
}