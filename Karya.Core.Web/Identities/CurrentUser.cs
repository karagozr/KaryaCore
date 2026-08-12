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

    // Language is a non-sensitive UI preference, so it is read per-request:
    // 1) "LanguageId" header  -> allows instant runtime switching (no new token)
    // 2) "lang" token claim   -> user's persisted default preference
    // 3) system default
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
