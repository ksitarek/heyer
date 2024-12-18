using Microsoft.AspNetCore.Authorization;

namespace Heyer.BuildingBlocks.Application.Authorization;

public class HasPermissionAuthorizationHandler : AuthorizationHandler<HasPermissionAuthorizationRequirement>
{
    private readonly IUserPermissionChecker _userPermissionChecker;

    public HasPermissionAuthorizationHandler(IUserPermissionChecker userPermissionChecker)
    {
        _userPermissionChecker = userPermissionChecker;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionAuthorizationRequirement requirement)
    {
        var checkResult = await _userPermissionChecker.HasPermissionAsync(requirement.PermissionName);

        if (checkResult)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}