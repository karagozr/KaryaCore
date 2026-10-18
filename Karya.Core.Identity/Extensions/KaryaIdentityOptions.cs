using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Server;

namespace Karya.Core.Identity.Extensions;

/// <summary>
/// Karya Identity kütüphanesinin tüm ayarları. "KaryaIdentity" konfigürasyon bölümünden okunur,
/// ardından <c>AddKaryaIdentity(..., o => {...})</c> delegesi ile kod üzerinden ezilebilir.
/// </summary>
public class KaryaIdentityOptions
{
    public const string SectionName = "KaryaIdentity";

    /// <summary>DbContext sağlayıcı yapılandırması (UseSqlServer, UseNpgsql vb.). Zorunludur.</summary>
    public Action<IServiceProvider, DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

    /// <summary>ASP.NET Core Identity ayarları (şifre politikası, lockout vb.).</summary>
    public Action<IdentityOptions>? ConfigureIdentity { get; set; }

    /// <summary>Kütüphane controller'ları otomatik eklensin mi?</summary>
    public bool EnableControllers { get; set; } = true;

    /// <summary>OpenIddict validation şemasını varsayılan authentication şeması yap.</summary>
    public bool UseAsDefaultAuthenticationScheme { get; set; } = true;

    /// <summary><c>UseKaryaIdentity</c> çağrısında bekleyen migration'lar uygulansın mı?</summary>
    public bool ApplyMigrationsOnStartup { get; set; }

    /// <summary><c>UseKaryaIdentity</c> çağrısında seeder'lar çalıştırılsın mı?</summary>
    public bool RunSeedersOnStartup { get; set; }

    public KaryaTokenOptions Token { get; } = new();

    public KaryaSeedOptions Seed { get; } = new();

    internal List<Action<IServiceCollection>> ServiceOverrides { get; } = [];

    /// <summary>DbContext sağlayıcısını belirler.</summary>
    public KaryaIdentityOptions UseDbContext(Action<DbContextOptionsBuilder> configure)
    {
        ConfigureDbContext = (_, builder) => configure(builder);
        return this;
    }

    /// <summary>Kütüphanenin varsayılan servis implementasyonunu değiştirir.</summary>
    public KaryaIdentityOptions ReplaceService<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        ServiceOverrides.Add(services => services.Replace(ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), lifetime)));
        return this;
    }

    /// <summary>Servis koleksiyonuna ek kayıt yapar (kütüphane kayıtlarından sonra çalışır).</summary>
    public KaryaIdentityOptions ConfigureServices(Action<IServiceCollection> configure)
    {
        ServiceOverrides.Add(configure);
        return this;
    }
}

public class KaryaTokenOptions
{
    public string[] TokenEndpoints { get; set; } = ["/connect/token", "/api/auth/login"];

    public TimeSpan? AccessTokenLifetime { get; set; }

    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);

    public bool AllowPasswordFlow { get; set; } = true;

    public bool AllowRefreshTokenFlow { get; set; } = true;

    public bool AcceptAnonymousClients { get; set; } = true;

    /// <summary>JSON gövdeli token isteklerini destekle.</summary>
    public bool EnableJsonTokenRequests { get; set; } = true;

    /// <summary>Geliştirme sertifikalarını kullan. Production'da kapatıp <see cref="ConfigureServer"/> ile gerçek sertifika verin.</summary>
    public bool UseDevelopmentCertificates { get; set; } = true;

    /// <summary>Access token şifrelemesini kapat (JWT'nin okunabilir olması için).</summary>
    public bool DisableAccessTokenEncryption { get; set; }

    /// <summary>OpenIddict server için ek yapılandırma.</summary>
    public Action<OpenIddictServerBuilder>? ConfigureServer { get; set; }
}

public class KaryaSeedOptions
{
    /// <summary>Varsayılan tenant/rol/admin seed'i yapılsın mı?</summary>
    public bool Enabled { get; set; } = true;

    public string TenantId { get; set; } = "BASE";

    public string TenantName { get; set; } = "Default";

    public string AdminUserName { get; set; } = "admin";

    public string AdminEmail { get; set; } = "admin@mail.com";

    public string AdminPassword { get; set; } = "Admin123*";
}
