using System.Text.Json.Serialization;

namespace GoodreadsScrapper.Models.JsonLd
{
    public class JsonLdBook
    {
        [JsonPropertyName("@context")]
        public required string Context { get; set; }

        [JsonPropertyName("@type")]
        public required string Type { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("bookFormat")]
        public string? BookFormat { get; set; }

        [JsonPropertyName("numberOfPages")]
        public int? NumberOfPages { get; set; }

        [JsonPropertyName("inLanguage")]
        public string? InLanguage { get; set; }

        [JsonPropertyName("isbn")]
        public string? Isbn { get; set; }

        [JsonPropertyName("author")]
        public List<JsonLdAuthor> Author { get; set; } = [];

        [JsonPropertyName("aggregateRating")]
        public JsonLdAggregateRating? AggregateRating { get; set; }
    }
    
    public class JsonLdAggregateRating
    {
        [JsonPropertyName("@type")]
        public required string Type { get; set; }

        [JsonPropertyName("ratingValue")]
        public double? RatingValue { get; set; }

        [JsonPropertyName("ratingCount")]
        public int? RatingCount { get; set; }

        [JsonPropertyName("reviewCount")]
        public int? ReviewCount { get; set; }
    }

    public class JsonLdAuthor
    {
        [JsonPropertyName("@type")]
        public required string Type { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

}