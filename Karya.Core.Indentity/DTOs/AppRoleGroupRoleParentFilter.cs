using Karya.Core.Interfaces.Filters;

namespace Karya.Core.Indentity.DTOs;

public class AppRoleGroupRoleParentFilter : IParentFilter
{
    public Guid RoleGroupId { get; set; }
}