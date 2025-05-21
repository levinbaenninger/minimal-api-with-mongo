using MongoDB.Bson.Serialization.Attributes;

namespace Api.Models;

public class Movie
{
    [BsonId]
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string Summary { get; set; } = null!;
    public string[] Actors { get; set; } = [];
}
