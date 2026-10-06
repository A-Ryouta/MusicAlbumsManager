namespace MusicAlbumsManager.Api.Domain.Entities;

public class User : ITrackableEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Album> Albums { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
