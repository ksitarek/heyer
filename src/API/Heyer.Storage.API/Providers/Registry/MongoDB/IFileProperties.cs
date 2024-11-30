namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public interface IFileProperties
{
    public string Key { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
}