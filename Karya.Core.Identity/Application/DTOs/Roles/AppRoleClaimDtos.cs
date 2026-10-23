using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Identity.Application.DTOs.Roles;

public class AppRoleClaimSDto : IByKeyDto
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
