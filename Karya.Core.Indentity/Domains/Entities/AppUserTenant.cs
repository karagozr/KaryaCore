namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// Kullanıcı ile tenant arasındaki üyelik ilişkisi. Bir admin birden çok
/// tenanta üye olabilir; normal kullanıcı yalnızca bir tenanta üyedir.
/// Kullanıcı giriş anında yalnızca üye olduğu tenant'lardan birini seçebilir.
/// </summary>
public class AppUserTenant
{
    public Guid UserId { get; set; }

    public string TenantId { get; set; } = null!;

    public AppUser? User { get; set; }
}
