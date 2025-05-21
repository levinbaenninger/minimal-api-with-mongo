using Api.Models;

namespace Api.Services;

public interface IMovieService
{
    Task<List<Movie>> GetAllMoviesAsync();
    Task<Movie> GetMovieByIdAsync(string id);
    Task CreateMovieAsync(Movie movie);
    Task UpdateMovieAsync(string id, Movie movie);
    Task DeleteMovieAsync(string id);
}
