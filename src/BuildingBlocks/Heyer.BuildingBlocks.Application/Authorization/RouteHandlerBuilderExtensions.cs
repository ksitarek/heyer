using Microsoft.AspNetCore.Builder;

namespace Heyer.BuildingBlocks.Application.Authorization;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequirePermission(
        this RouteHandlerBuilder builder, string permissionName)
    {
        builder.RequireAuthorization(x =>
        {
            x.RequireAuthenticatedUser();
            x.Requirements.Add(new HasPermissionAuthorizationRequirement(permissionName));
        });

        return builder;
    }
}