namespace Karya.Core.App.Interfaces.Services;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string permission);
}