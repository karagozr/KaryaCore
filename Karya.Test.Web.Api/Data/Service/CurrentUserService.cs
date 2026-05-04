using Karya.Core.App.Interfaces.Services;

namespace Karya.Test.Web.Api.Data.Service;

public class CurrentUserService : ICurrentUserService
{
    public string UserId { get; } = "sys_admin";
}