namespace Heyer.BuildingBlocks.Application.Authorization;

public interface IUserPermissionChecker
{
    Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
}