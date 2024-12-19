namespace Heyer.Storage.API.Tests.Utils.Validators;

internal interface IStorageStrategyValidator
{
    Task ValidateFileIsNotPresent(string key);
    Task ValidateFileIsPresent(string key);
    Task ValidateFileIsPreserved(string key);
}