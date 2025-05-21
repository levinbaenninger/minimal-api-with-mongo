using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Api.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

var app = builder.Build();

app.MapGet("/check", async (IOptions<DatabaseSettings> options) =>
{
    try
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var databases = await mongoClient.ListDatabaseNamesAsync();
        var databaseList = await databases.ToListAsync();
        return Results.Ok(databaseList);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
