using Microsoft.AspNetCore.Authorization;

namespace Heyer.BuildingBlocks.Application.Authorization;

public class HasPermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public HasPermissionAuthorizationRequirement(string permissionName) => PermissionName = permissionName;
    public string PermissionName { get; }
}