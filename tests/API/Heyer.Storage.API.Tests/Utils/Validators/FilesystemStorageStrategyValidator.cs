using Shouldly;
using Heyer.Storage.API.Providers.Storage.Filesystem;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Tests.Utils.Validators;

internal class FilesystemStorageStrategyValidator : IStorageStrategyValidator
{
    private readonly IOptions<FilesystemStorageOptions> _options;

    public FilesystemStorageStrategyValidator(IOptions<FilesystemStorageOptions> options) => _options = options;

    public Task ValidateFileIsNotPresent(string key)
    {
        File.Exists($"{_options.Value.RootPath}/{key}")
            .ShouldBeFalse();

        return Task.CompletedTask;
    }

    public Task ValidateFileIsPresent(string key)
    {
        File.Exists($"{_options.Value.RootPath}/{key}")
            .ShouldBeTrue();

        return Task.CompletedTask;
    }

    public Task ValidateFileIsPreserved(string key) => ValidateFileIsPresent(key);
}