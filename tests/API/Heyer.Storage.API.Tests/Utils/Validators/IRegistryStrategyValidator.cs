namespace Heyer.Storage.API.Tests.Utils.Validators;

internal interface IRegistryStrategyValidator
{
    Task ValidateFilePropertiesAsync(string key, string expectedFileName, string expectedContentType, int expectedSize);
    Task ValidateFileIsPreserved(string key);
    Task ValidateFileIsNotPresent(string key);
}