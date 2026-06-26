using Microsoft.AspNetCore.Http;
using Karya.Core.Interfaces.Identities;
using System.Security.Claims;

namespace Karya.Core.Web.Identities;

public class CurrentUser : ICurrentUser
{

    private readonly IHttpContextAccessor _accessor;

    private const string DefaultLanguage = "en";

    public string UserId => "USR01";//_accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

    public string TenantId => "COMP0001"; //_accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TanentId").Value;

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

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

}
