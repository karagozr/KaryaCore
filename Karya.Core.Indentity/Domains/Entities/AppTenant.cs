using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// Tenant (kiracı) kaydı. Tenant verisi yalnızca Sistem Admin tarafından
/// girilebilir. Id, tenant kodudur (AppUser.TenantId ile eşleşir).
/// </summary>
public class AppTenant : IBaseEntity<string>
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
