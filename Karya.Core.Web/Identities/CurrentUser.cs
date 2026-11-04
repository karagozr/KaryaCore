using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Karya.Core.Web.Identities;

public class CurrentUser : ICurrentUser
{

    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private const string DefaultLanguage = "en";

    public string UserId =>
    _accessor.HttpContext?.User?.FindFirst("UserId")?.Value
    ?? _accessor.HttpContext?.User?.FindFirst("sub")?.Value
    ?? _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
    ?? string.Empty;

    public string TenantId =>
        _accessor.HttpContext?.User.FindFirst("TenantId")?.Value
        ?? string.Empty;

    public string UserName =>
        _accessor.HttpContext?.User.FindFirst("UserName")?.Value
        ?? _accessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
        ?? string.Empty;

    public string LanguageId
    {
        get
        {
            var ctx = _accessor.HttpContext;

            var fromHeader = ctx?.Request.Headers["LanguageId"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(fromHeader))
                return fromHeader.Trim().ToLowerInvariant();

            var fromClaim = ctx?.User?.FindFirst("lang")?.Value;
            if (!string.IsNullOrWhiteSpace(fromClaim))
                return fromClaim.Trim().ToLowerInvariant();

            return DefaultLanguage;
        }
    }
}
