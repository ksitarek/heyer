using Microsoft.AspNetCore.Http;

namespace Heyer.BuildingBlocks.Application.Authorization;

internal class ClaimsPermissionChecker : IUserPermissionChecker
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsPermissionChecker(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public Task<bool> HasPermissionAsync(
        string permissionName,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var claims = httpContext?.User.Claims;
        var hasPermission = claims?.Any(c => c.Type == "permissions" && c.Value == permissionName);
        return Task.FromResult(hasPermission ?? false);
    }
}