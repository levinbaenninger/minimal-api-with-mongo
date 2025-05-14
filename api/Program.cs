using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IMongoClient>(sp => {
    var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

var app = builder.Build();

app.MapGet("/databases", async (IMongoClient mongoClient) =>
{
    try
    {
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
