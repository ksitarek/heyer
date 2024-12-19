using FileSignatures;

namespace Heyer.Storage.API.Validators;

public static class FormFileExtensions
{
    private static readonly FileFormatInspector FileInspector = new();

    public static FileFormat? GetFileFormat(this IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileFormat = FileInspector.DetermineFileFormat(fileStream);
        return fileFormat;
    }
}