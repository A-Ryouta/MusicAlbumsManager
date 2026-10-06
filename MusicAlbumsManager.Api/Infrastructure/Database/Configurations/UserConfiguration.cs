using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicAlbumsManager.Api.Domain.Entities;

namespace MusicAlbumsManager.Api.Infrastructure.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedOnAdd();
        builder.Property(user => user.Name).IsRequired();
        builder.Property(user => user.CreatedAt).IsRequired();

        builder.HasMany(user => user.Albums)
            .WithOne()
            .HasForeignKey(album => album.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
