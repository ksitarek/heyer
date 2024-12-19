namespace Heyer.Storage.API.Tests.Utils.Validators;

internal interface IRegistryStrategyValidator
{
    Task ValidateFileIsNotPresent(string key);
    Task ValidateFileIsPreserved(string key);
    Task ValidateFilePropertiesAsync(string key, string expectedFileName, string expectedContentType, int expectedSize);
}