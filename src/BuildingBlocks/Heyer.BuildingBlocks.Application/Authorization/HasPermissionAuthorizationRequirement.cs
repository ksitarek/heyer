using Microsoft.AspNetCore.Authorization;

namespace Heyer.BuildingBlocks.Application.Authorization;

public class HasPermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }

    public HasPermissionAuthorizationRequirement(string permissionName)
    {
        PermissionName = permissionName;
    }
}