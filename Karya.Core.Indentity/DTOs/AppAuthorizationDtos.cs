namespace Karya.Core.Indentity.DTOs;

/// <summary>OpenIddict authorization (yetkilendirme) listeleme görünümü.</summary>
public class AppAuthorizationLDto
{
    public Guid Id { get; set; }
    public Guid? ApplicationId { get; set; }
    public string? Subject { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public DateTime? CreationDate { get; set; }
}
