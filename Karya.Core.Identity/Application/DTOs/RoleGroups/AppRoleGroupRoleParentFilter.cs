using Karya.Core.Interfaces.Filters;

namespace Karya.Core.Identity.Application.DTOs.RoleGroups;

public class AppRoleGroupRoleParentFilter : IParentFilter
{
    public Guid RoleGroupId { get; set; }
}