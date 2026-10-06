using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MusicAlbumsManager.Api.Infrastructure.Database;
using System.ComponentModel.DataAnnotations;

namespace MusicAlbumsManager.Api.Features.Albums;

public static class RemoveAlbums
{
    public sealed record Request([Required] string UserName, [Required] ICollection<string> AlbumNames);
    public sealed record Response(int RemovedCount);

    public static async Task<IResult> Handle(
        string userName,
        ICollection<string> albumNames,
        IValidator<Request> validator,
        MusicLibraryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var request = new Request(userName, albumNames);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var user = await dbContext.Users.FirstOrDefaultAsync(user => string.Equals(user.Name, userName, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

        if (user is null)
            return Results.NotFound("User not found.");

        var albums = await dbContext.Albums
            .Where(album => album.UserId == user.Id && albumNames.Contains(album.Name))
            .ToListAsync(cancellationToken);

        if (albums.Count > 0)
        {
            dbContext.Albums.RemoveRange(albums);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Results.Ok(new Response(albums.Count));
    }
}