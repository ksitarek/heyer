using FileSignatures;
using FileSignatures.Formats;
using FluentValidation;

namespace Heyer.Storage.API.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
    private static readonly Type[] AllowedFormats = { typeof(Image), typeof(Pdf) };
    
    private static readonly FileFormatInspector FileInspector = new();
    
    public FileValidator()
    {
        RuleFor(x => x.Length)
            .GreaterThan(0)
            .WithMessage("File is empty.")
            .LessThanOrEqualTo(10 * 1024 * 1024)
            .WithMessage("Max file size is 10MB.");
        
        RuleFor(x => x.FileName)
            .NotNull()
            .Must(x => AllowedExtensions.Contains(Path.GetExtension(x).ToLower()))
            .WithMessage("Invalid file extension.");
        
        RuleFor(x => x)
            .Must(BeOfAllowedType)
            .WithMessage("Invalid file format.");
    }

    private static bool BeOfAllowedType(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileFormat = FileInspector.DetermineFileFormat(fileStream);
        return fileFormat != null && AllowedFormats.Any(t => fileFormat.GetType().IsAssignableTo(t));
    }
}