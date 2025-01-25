using Microsoft.EntityFrameworkCore;

namespace Heyer.Storage.API.Providers.Registry.Npgsql;

internal class StorageDbContext : DbContext
{
    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
    {
    }

    public DbSet<StorageRegistryEntry> StorageRegistryEntries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new StorageRegistryEntityTypeConfiguration());
    }
}