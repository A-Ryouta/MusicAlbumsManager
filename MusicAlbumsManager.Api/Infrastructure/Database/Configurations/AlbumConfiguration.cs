using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicAlbumsManager.Api.Domain.Entities;

namespace MusicAlbumsManager.Api.Infrastructure.Database.Configurations;

public sealed class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.HasKey(album => album.Id);
        builder.Property(album => album.Id).ValueGeneratedOnAdd();
        builder.Property(album => album.Name).IsRequired();
        builder.Property(album => album.ArtistName).IsRequired();
        builder.Property(album => album.Url).IsRequired();
        builder.Property(album => album.Cover).IsRequired(false);
        builder.Property(album => album.CreatedAt).IsRequired();
        builder.Property(album => album.UserId).IsRequired();
    }
}
