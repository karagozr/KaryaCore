using Microsoft.AspNetCore.Http;
using Karya.Core.Interfaces.Identities;
using System.Security.Claims;

namespace Karya.Core.Web.Identities;

public class CurrentUser : ICurrentUser
{

    private readonly IHttpContextAccessor _accessor;

    public string UserId { get; } = string.Empty;

    public string TanentId { get; } = string.Empty;

    //public CurrentUser()
    //{
    //    UserId = "sys_admin";
    //    TanentId = "COMP02";
    //}
    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        var user = _accessor.HttpContext?.User;
        UserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        TanentId = user.Claims.FirstOrDefault(c => c.Type == "TanentId").Value;
    }

}
