using Microsoft.AspNetCore.Http;

namespace Heyer.BuildingBlocks.Application.Authorization;

internal class ClaimsPermissionChecker : IUserPermissionChecker
{
    private readonly HttpContextAccessor _httpContextAccessor;

    public ClaimsPermissionChecker(HttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Task<bool> HasPermissionAsync(
        string permissionName, 
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var claims = httpContext?.User.Claims;
        var hasPermission = claims?.Any(c => c.Type == permissionName);
        return Task.FromResult(hasPermission ?? false);
    }
}