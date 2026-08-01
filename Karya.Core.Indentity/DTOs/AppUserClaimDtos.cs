using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

public class AppUserClaimSDto : ISingleDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

public class AppUserClaimLDto : ISelectDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

public class AppUserClaimADto : IInsertDto
{
    public Guid UserId { get; set; }
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}

public class AppUserClaimUDto : IUpdateDto
{
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
}
