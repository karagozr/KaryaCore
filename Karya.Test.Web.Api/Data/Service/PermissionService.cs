using Karya.Core.App.Interfaces.Services;

namespace Karya.Test.Web.Api.Data.Service;

public class PermissionService : IPermissionService
{
    public Task<bool> HasPermissionAsync(string userId, string permission)
    {
        if (userId == "sys_admin")
            return Task.FromResult(true);

        return Task.FromResult(false);
    }
}