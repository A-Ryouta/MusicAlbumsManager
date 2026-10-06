using Microsoft.EntityFrameworkCore;
using MusicAlbumsManager.Api.Domain;
using MusicAlbumsManager.Api.Domain.Entities;
using MusicAlbumsManager.Api.Interfaces;
using System.Reflection;
using System.Reflection.Emit;

namespace MusicAlbumsManager.Api.Infrastructure.Database
{
    public class MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options, IDateTimeAccessor dateTimeAccessor) : DbContext(options)
    {
        private readonly IDateTimeAccessor _dateTimeAccessor = dateTimeAccessor;

        public DbSet<User> Users => Set<User>();
        public DbSet<Album> Albums => Set<Album>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<ITrackableEntity>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.CreatedAt = _dateTimeAccessor.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
