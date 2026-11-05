using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

public class AppRoleGroupRoleSDto : IByKeyDto
{
    public Guid Id { get; set; }
    public Guid RoleGroupId { get; set; }
    public Guid RoleId { get; set; }
}

public class AppRoleGroupRoleLDto : ISelectDto
{
    public Guid Id { get; set; }
    public Guid RoleGroupId { get; set; }
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }
}

public class AppRoleGroupRoleADto : IInsertDto
{
    public Guid RoleId { get; set; }
}

public class AppRoleGroupRoleUDto : IUpdateDto
{
    public Guid? RoleId { get; set; }
}