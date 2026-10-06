using FluentValidation;
using MusicAlbumsManager.Api.Domain.Entities;
using MusicAlbumsManager.Api.Interfaces;

namespace MusicAlbumsManager.Api.Features.Albums;

public class SearchAlbums
{
    public record Request(string? Name, string? ArtistName);
    public record Response(ICollection<Album> Albums);

    public static class Endpoint
    {
        public static async Task<IResult> Handle(
            string? name,
            string? artistName,
            IValidator<Request> validator,
            IMusicLibrarySource musicLibrarySource,
            CancellationToken cancellationToken)
        {
            var request = new Request(name, artistName);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            var albums = await musicLibrarySource.GetAlbumsAsync(request.Name, request.ArtistName, cancellationToken);
            return Results.Ok(new Response(albums));
        }
    }
}
