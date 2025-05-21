using Api.Settings;
using Api.Models;
using Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IMovieService, MongoMovieService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/api/movies", async (IMovieService movieService) => {
    return Results.Ok(await movieService.GetAllMoviesAsync());
});

app.MapGet("/api/movies/{id}", async (IMovieService movieService, string id) => {
    var movie = await movieService.GetMovieByIdAsync(id);

    if (movie is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(movie);
});

app.MapPost("/api/movies", async (IMovieService movieService, Movie movie) => {
    await movieService.CreateMovieAsync(movie);
    return Results.Created($"/api/movies/{movie.Id}", movie);
});

app.MapPut("/api/movies/{id}", async (IMovieService movieService, string id, Movie movie) => {
    var existingMovie = await movieService.GetMovieByIdAsync(id);

    if (existingMovie is null)
    {
        return Results.NotFound();
    }

    movie.Id = existingMovie.Id;

    await movieService.UpdateMovieAsync(id, movie);
    return Results.Ok(movie);
});

app.MapDelete("/api/movies/{id}", async (IMovieService movieService, string id) => {
    var existingMovie = await movieService.GetMovieByIdAsync(id);

    if (existingMovie is null)
    {
        return Results.NotFound();
    }

    await movieService.DeleteMovieAsync(id);
    return Results.NoContent();
});

app.Run();
