namespace Heyer.Storage.API.Tests.Utils.Validators;

internal interface IStorageStrategyValidator
{
    Task ValidateFileIsPresent(string key);
    Task ValidateFileIsPreserved(string key);
    Task ValidateFileIsNotPresent(string key);
}