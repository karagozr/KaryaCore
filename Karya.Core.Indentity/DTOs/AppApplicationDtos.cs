namespace Karya.Core.Indentity.DTOs;

/// <summary>OpenIddict uygulama (client) görünümü.</summary>
public class AppApplicationLDto
{
    public string? Id { get; set; }
    public string? ClientId { get; set; }
    public string? DisplayName { get; set; }
    public string? ClientType { get; set; }
}

/// <summary>OpenIddict uygulama (client) oluşturma isteği.</summary>
public class AppApplicationADto
{
    public string ClientId { get; set; } = null!;
    public string? ClientSecret { get; set; }
    public string? DisplayName { get; set; }
    /// <summary>"public" veya "confidential".</summary>
    public string? ClientType { get; set; }
    public List<string> RedirectUris { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}
