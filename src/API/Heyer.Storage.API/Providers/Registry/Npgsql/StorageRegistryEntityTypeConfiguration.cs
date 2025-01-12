using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Heyer.Storage.API.Providers.Registry.Npgsql;

public class StorageRegistryEntityTypeConfiguration : IEntityTypeConfiguration<StorageRegistryEntry>
{
    public void Configure(EntityTypeBuilder<StorageRegistryEntry> builder)
    {
        builder.HasKey(x => x.Key);
        builder.Property(x => x.ContentType).IsRequired();
        builder.Property(x => x.FileName).IsRequired();
        builder.Property(x => x.Size).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Preserve).IsRequired();
    }
}