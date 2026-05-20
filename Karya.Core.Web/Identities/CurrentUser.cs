using Microsoft.AspNetCore.Http;
using Karya.Core.Interfaces.Identities;
using System.Security.Claims;

namespace Karya.Core.Web.Identities;

public class CurrentUser : ICurrentUser
{

    private readonly IHttpContextAccessor _accessor;

    public string UserId => _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

    public string TenantId => _accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TanentId").Value;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

}
