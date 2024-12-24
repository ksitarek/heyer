using FileSignatures;

namespace Heyer.Storage.API.Validators;

public static class FormFileExtensions
{
    private static readonly FileFormatInspector _fileInspector = new();

    public static FileFormat? GetFileFormat(this IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileFormat = _fileInspector.DetermineFileFormat(fileStream);
        return fileFormat;
    }
}