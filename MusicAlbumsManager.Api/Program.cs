using Microsoft.EntityFrameworkCore;
using MusicAlbumsManager.Api.Features.Albums;
using MusicAlbumsManager.Api.Infrastructure.Database;
using MusicAlbumsManager.Api.Infrastructure.Deezer;
using MusicAlbumsManager.Api.Interfaces;
using MusicAlbumsManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("music-library-db");
builder.Services.AddDbContext<MusicLibraryDbContext>(options => options.UseNpgsql(connectionString));
builder.EnrichNpgsqlDbContext<MusicLibraryDbContext>();

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDateTimeAccessor, DateTimeUtcAccessor>();
builder.Services.AddHttpClient<IMusicLibrarySource, DeezerMusicLibrarySource>(client =>
{
    client.BaseAddress = new Uri("https://api.deezer.com/");
});

var app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.MapAlbumsEndpoints();

app.Run();
