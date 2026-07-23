using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

public class AppRoleGroupSDto : ISingleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TenantId { get; set; }
}

public class AppRoleGroupLDto : ISelectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? TenantId { get; set; }
}

public class AppRoleGroupADto : IInsertDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TenantId { get; set; }
}

public class AppRoleGroupUDto : IUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TenantId { get; set; }
}
