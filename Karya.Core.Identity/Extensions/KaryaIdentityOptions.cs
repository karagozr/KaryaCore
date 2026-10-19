using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Karya.Core.Identity.Extensions;

public class KaryaIdentityOptions
{
    public const string SectionName = "KaryaIdentity";

    /// DbContext sağlayıcı yapılandırması (UseSqlServer, UseNpgsql vb.). Zorunludur.
    public Action<IServiceProvider, DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

    /// ASP.NET Core Identity ayarları (şifre politikası vs.)
    public Action<IdentityOptions>? ConfigureIdentity { get; set; }

    /// Kütüphane controller'ları otomatik eklensin mi?
    public bool EnableControllers { get; set; } = true;

    /// OpenIddict validation şemasını varsayılan authentication şeması yap.
    public bool UseAsDefaultAuthenticationScheme { get; set; } = true;

    /// UseKaryaIdentity çağrıldığı zaman bekleyen migration'lar uygulansın mı?
    public bool ApplyMigrationsOnStartup { get; set; }

    /// UseKaryaIdentity çağrıldığı zaman seeder'lar çalıştırılsın mı?
    public bool RunSeedersOnStartup { get; set; }

    public KaryaTokenOptions Token { get; } = new();

    public KaryaSeedOptions Seed { get; } = new();

    internal List<Action<IServiceCollection>> ServiceOverrides { get; } = [];

    /// DbContext sağlayıcısını belirler.
    public KaryaIdentityOptions UseDbContext(Action<DbContextOptionsBuilder> configure)
    {
        ConfigureDbContext = (_, builder) => configure(builder);
        return this;
    }

    /// Kütüphanenin varsayılan servis implementasyonunu değiştirir.
    public KaryaIdentityOptions ReplaceService<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        ServiceOverrides.Add(services => services.Replace(ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), lifetime)));
        return this;
    }

    /// Servislere ek kayıt yapar (kütüphane kayıtlarından sonra çalışır).
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

    /// JSON gövdeli token isteklerini destekle.
    public bool EnableJsonTokenRequests { get; set; } = true;

    /// Geliştirme sertifikalarını kullan. Production'da kapatıp <see cref="ConfigureServer"/> ile gerçek sertifika verin.
    public bool UseDevelopmentCertificates { get; set; } = true;

    /// Access token şifrelemesini kapat (JWT'nin okunabilir olması için).
    public bool DisableAccessTokenEncryption { get; set; }

    /// HTTPS zorunluluğunu kaldır (yalnızca geliştirme ortamında HTTP üzerinden login için).
    public bool DisableTransportSecurityRequirement { get; set; }

    /// OpenIddict server için ek yapılandırma.
    public Action<OpenIddictServerBuilder>? ConfigureServer { get; set; }
}

public class KaryaSeedOptions
{
    /// Varsayılan tenant/rol/admin seed'i yapılsın mı?
    public bool Enabled { get; set; } = true;

    public string TenantId { get; set; } = "BASE";

    public string TenantName { get; set; } = "Default";

    public string AdminUserName { get; set; } = "admin";

    public string AdminEmail { get; set; } = "admin@mail.com";

    public string AdminPassword { get; set; } = "Admin123*";
}
