using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

public class AppRoleClaimSDto : ISingleDto
{
    public int Id { get; set; }
    public Guid RoleId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

public class AppRoleClaimLDto : ISelectDto
{
    public int Id { get; set; }
    public Guid RoleId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

public class AppRoleClaimADto : IInsertDto
{
    public Guid RoleId { get; set; }
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}

public class AppRoleClaimUDto : IUpdateDto
{
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}
