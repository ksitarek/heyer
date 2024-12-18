using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Heyer.BuildingBlocks.Application.Authorization;

internal class ClaimsUserDataProvider : IUserDataProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsUserDataProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
    public Guid CompanyId => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue("CompanyId") ?? Guid.Empty.ToString());
    public string CompanyName => _httpContextAccessor.HttpContext?.User.FindFirstValue("CompanyName") ?? string.Empty;
}