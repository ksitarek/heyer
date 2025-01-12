namespace Heyer.Storage.API.Providers.Registry.Npgsql;

public class StorageRegistryEntry : IFileProperties
{
    public string ContentType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string FileName { get; set; } = null!;
    public string Key { get; set; } = null!;
    public bool Preserve { get; set; }
    public long Size { get; set; }
}