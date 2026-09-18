using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcıyı rol grubuna atama/kaldırma isteği.</summary>
public class AppUserRoleGroupSDto:IByKeyDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleGroupId { get; set; }
}

public class AppUserRoleGroupLDto:ISelectDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public Guid RoleGroupId { get; set; }
    public string RoleGroupName { get; set; } = null!;
}

public class AppUserRoleGroupADto : IInsertDto
{
    public Guid UserId { get; set; }
    public Guid RoleGroupId { get; set; }
}

public class AppUserRoleGroupUDto : IUpdateDto
{
    public Guid UserId { get; set; }
    public Guid RoleGroupId { get; set; }
}

