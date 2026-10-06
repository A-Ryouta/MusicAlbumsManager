using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MusicAlbumsManager.Api.Domain.Entities;
using MusicAlbumsManager.Api.Infrastructure.Database;

namespace MusicAlbumsManager.Api.Features.Albums;

public static class AddAlbums
{
    public sealed record Request(string UserName, ICollection<Album> Albums);
    public sealed record Response(IReadOnlyCollection<Album> Albums);

    public static async Task<IResult> Handle(
        Request request,
        IValidator<Request> validator,
        MusicLibraryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var user = await dbContext.Users.FirstOrDefaultAsync(user => string.Equals(user.Name, request.UserName, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

        if (user is null)
            return Results.NotFound("User not found.");

        var albumNames = request.Albums.Select(album => album.Name).ToList();

        var existingAlbums = await dbContext.Albums
            .Where(album => album.UserId == user.Id && albumNames.Contains(album.Name))
            .ToListAsync(cancellationToken);

        var albumsToAdd = request.Albums
            .Where(album => !existingAlbums.Any(existing => existing.Name == album.Name))
            .ToArray();

        if (albumsToAdd.Length > 0)
        {
            dbContext.Albums.AddRange(albumsToAdd);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Results.Ok(new Response(albumsToAdd));
    }
}