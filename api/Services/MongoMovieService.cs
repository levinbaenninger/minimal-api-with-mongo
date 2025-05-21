using Api.Models;
using Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Services;

public class MongoMovieService : IMovieService
{
    private readonly IMongoCollection<Movie> _moviesCollection;

    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _moviesCollection = database.GetCollection<Movie>(options.Value.MoviesCollectionName);
    }

    public async Task CreateMovieAsync(Movie movie)
    {
        await _moviesCollection.InsertOneAsync(movie);
    }

    public async Task DeleteMovieAsync(string id)
    {
        var result = await _moviesCollection.DeleteOneAsync(m => m.Id == id);
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await _moviesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Movie> GetMovieByIdAsync(string id)
    {
        return await _moviesCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateMovieAsync(string id, Movie movie)
    {
        await _moviesCollection.ReplaceOneAsync(m => m.Id == id, movie);
    }
}
