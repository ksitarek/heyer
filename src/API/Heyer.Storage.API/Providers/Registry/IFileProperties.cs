namespace Heyer.Storage.API.Providers.Registry;

public interface IFileProperties
{
    public string ContentType { get; set; }
    public string FileName { get; set; }
    public string Key { get; set; }
    public long Size { get; set; }
}