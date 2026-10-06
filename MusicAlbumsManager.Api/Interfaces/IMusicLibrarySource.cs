using MusicAlbumsManager.Api.Domain.Entities;

namespace MusicAlbumsManager.Api.Interfaces;

public interface IMusicLibrarySource
{
    Task<ICollection<Album>> GetAlbumsAsync(string? name, string? artistName, CancellationToken cancellationToken = default);
}
