namespace MusicAlbumsManager.Api.Domain.Entities;

public class Album : ITrackableEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string ArtistName { get; set; }
    public required string Url { get; set; }
    public string? Cover { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
