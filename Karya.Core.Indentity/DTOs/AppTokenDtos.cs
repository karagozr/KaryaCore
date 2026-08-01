namespace Karya.Core.Indentity.DTOs;

/// <summary>OpenIddict token listeleme görünümü.</summary>
public class AppTokenLDto
{
    public Guid Id { get; set; }
    public Guid? ApplicationId { get; set; }
    public Guid? AuthorizationId { get; set; }
    public string? Subject { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public string? ReferenceId { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? RedemptionDate { get; set; }
}
