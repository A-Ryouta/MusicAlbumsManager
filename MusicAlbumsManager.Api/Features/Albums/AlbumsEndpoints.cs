namespace MusicAlbumsManager.Api.Features.Albums;

public static class AlbumsEndpoints
{
    public static void MapAlbumsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/albums")
        .WithTags("Grill Optimization");

        //Calculate optimal grill placement
        group.MapGet("/search", SearchAlbums.Endpoint.Handle)
            .WithName("SearchAlbums")
            .ProducesValidationProblem()
            .Produces<SearchAlbums.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Searches for albums based on the provided criteria and returns the matching results");

        app.MapPost("/{userName:string}", AddAlbums.Handle)
            .WithName("AddAlbumsToLibrary")
            .Produces<AddAlbums.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Adds one or more search results to the user's album library, ignoring albums already present.");

        app.MapDelete("/{userName:string}", RemoveAlbums.Handle)
            .WithName("RemoveAlbumsFromLibrary")
            .Produces<RemoveAlbums.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Removes one or more albums from the user's library by album name.");
    }
}
