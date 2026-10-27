using Karya.Core.App.Interfaces.Services;
using Karya.Core.Identity.Application.Seeders;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace Karya.Core.Identity.Extensions;

public static class CoreIdentityServiceCollectionExtensions
{
    /// Karya Identity'yi varsayılan AppUser / AppRole entity'leri ile ekler.
    public static IServiceCollection AddKaryaIdentity<TContext>( this IServiceCollection services,IConfiguration configuration, Action<CoreIdentityOptions>? configure = null)
        where TContext : AppDbContext
        => services.AddKaryaIdentity<TContext, AppUser, AppRole>(configuration, configure);

    /// Karya Identity için genişletilmiş kullanıcı/rol entity'leri eklenir.
    public static IServiceCollection AddKaryaIdentity<TContext, TUser, TRole>( this IServiceCollection services,IConfiguration configuration, Action<CoreIdentityOptions>? configure = null)
        where TContext : AppDbContext
        where TUser : AppUser, new()
        where TRole : AppRole, new()
    {
        void Configure(CoreIdentityOptions o)
        {
            configuration.GetSection(CoreIdentityOptions.SectionName).Bind(o);
            configure?.Invoke(o);
        }

        var options = new CoreIdentityOptions();
        Configure(options);

        if (options.ConfigureDbContext is null)
            throw new InvalidOperationException(
                $"{nameof(CoreIdentityOptions)}.{nameof(CoreIdentityOptions.UseDbContext)} ile bir veritabanı sağlayıcısı belirtilmelidir (ör. o.UseDbContext(b => b.UseSqlServer(...))).");

        services.AddOptions<CoreIdentityOptions>().Configure(Configure);
        services.AddSingleton(new KaryaIdentityContextDescriptor(typeof(TContext)));

        services.AddDbContext<TContext>(options.ConfigureDbContext);
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TContext>());
        services.TryAddScoped<IdentityUnitOfWork>();

        services.AddKaryaAspNetIdentity<TContext, TUser, TRole>(options);
        services.AddKaryaIdentityServices<TUser, TRole>();
        services.AddKaryaOpenIddict<TContext>(options.Token);
        services.AddKaryaAuthentication(options);
        services.AddKaryaIdentityControllers(options.EnableControllers);

        foreach (var serviceOverride in options.ServiceOverrides)
            serviceOverride(services);

        return services;
    }

    /// Ek bir veritabanı seeder'ı kaydeder
    public static IServiceCollection AddKaryaSeeder<TSeeder>(this IServiceCollection services) where TSeeder : class, IDatabaseSeeder
    {
        services.AddScoped<IDatabaseSeeder, TSeeder>();
        return services;
    }

    private static void AddKaryaAspNetIdentity<TContext, TUser, TRole>(this IServiceCollection services, CoreIdentityOptions options)
        where TContext : AppDbContext
        where TUser : AppUser
        where TRole : AppRole
    {
        void ConfigureIdentity(IdentityOptions o) => options.ConfigureIdentity?.Invoke(o);

        services.AddIdentity<TUser, TRole>(ConfigureIdentity)
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders();

        // Kütüphane servisleri temel tipler üzerinden çalışır; genişletilmiş tiplerde
        // aynı context üzerinde temel tip yöneticileri de kaydedilir (TPH sorguları türetilmiş tipleri de döndürür).
        if (typeof(TUser) != typeof(AppUser) || typeof(TRole) != typeof(AppRole))
        {
            services.AddIdentityCore<AppUser>(ConfigureIdentity)
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<TContext>()
                .AddSignInManager();
        }
    }

    private static void AddKaryaIdentityServices<TUser, TRole>(this IServiceCollection services)
        where TUser : AppUser, new()
        where TRole : AppRole, new()
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<ICurrentUser, CurrentUser>();
        services.TryAddSingleton<IIdentityEntityFactory, IdentityEntityFactory<TUser, TRole>>();

        services.TryAddScoped<IAppAuthService, AppAuthService>();
        services.TryAddScoped<AppUserService>();
        services.TryAddScoped<IAppUserService>(sp => sp.GetRequiredService<AppUserService>());
        services.TryAddScoped<IPermissionService, IdentityPermissionService>();

        services.TryAddScoped<AppUserTenantService>();
        services.TryAddScoped<AppRoleService>();
        services.TryAddScoped<AppRoleGroupService>();
        services.TryAddScoped<AppUserRoleService>();
        services.TryAddScoped<AppUserRoleGroupService>();
        services.TryAddScoped<AppRoleGroupRoleService>();
        services.TryAddScoped<AppRoleClaimService>();
        services.TryAddScoped<AppUserClaimService>();
        services.TryAddScoped<AppScopeService>();
        services.TryAddScoped<AppTenantService>();
        services.TryAddScoped<UserProfileService>();
        services.TryAddScoped<IAppProfileService, AppProfileService>();

        services.TryAddEnumerable(ServiceDescriptor.Scoped<IDatabaseSeeder, IdentityDataSeeder>());
    }

    private static void AddKaryaOpenIddict<TContext>(this IServiceCollection services, KaryaTokenOptions token)
        where TContext : AppDbContext
    {
        var extractPostRequestDescriptor = OpenIddictServerAspNetCoreHandlers
            .ExtractPostRequest<OpenIddictServerEvents.ExtractTokenRequestContext>.Descriptor;

        services.AddOpenIddict()
            .AddCore(o =>
            {
                o.UseEntityFrameworkCore().UseDbContext<TContext>()
                    .ReplaceDefaultEntities<AppApplication, AppAuthorization, AppScope, AppToken, Guid>();
            })
            .AddServer(o =>
            {
                o.SetTokenEndpointUris(token.TokenEndpoints);

                if (token.AllowPasswordFlow)
                    o.AllowPasswordFlow();

                if (token.AllowRefreshTokenFlow)
                    o.AllowRefreshTokenFlow();

                if (token.AccessTokenLifetime is not null)
                    o.SetAccessTokenLifetime(token.AccessTokenLifetime);

                o.SetRefreshTokenLifetime(token.RefreshTokenLifetime);

                if (token.AcceptAnonymousClients)
                    o.AcceptAnonymousClients();

                if (token.UseDevelopmentCertificates)
                    o.AddDevelopmentEncryptionCertificate()
                     .AddDevelopmentSigningCertificate();

                if (token.DisableAccessTokenEncryption)
                    o.DisableAccessTokenEncryption();

                var aspNetCore = o.UseAspNetCore()
                 .EnableTokenEndpointPassthrough();

                if (token.DisableTransportSecurityRequirement)
                    aspNetCore.DisableTransportSecurityRequirement();

                if (token.EnableJsonTokenRequests)
                {
                    o.RemoveEventHandler(extractPostRequestDescriptor);
                    o.AddEventHandler<OpenIddictServerEvents.ExtractTokenRequestContext>(builder =>
                    {
                        builder.UseScopedHandler<ExtractJsonTokenRequestHandler>();
                        builder.SetOrder(extractPostRequestDescriptor.Order);
                    });
                }

                token.ConfigureServer?.Invoke(o);
            })
            .AddValidation(o =>
            {
                o.UseLocalServer();
                o.UseAspNetCore();
            });
    }

    private static void AddKaryaAuthentication(this IServiceCollection services, CoreIdentityOptions options)
    {
        var authentication = services.AddAuthentication();

        if (!options.UseAsDefaultAuthenticationScheme)
            return;

        services.Configure<Microsoft.AspNetCore.Authentication.AuthenticationOptions>(o =>
        {
            o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
    }

    private static void AddKaryaIdentityControllers(this IServiceCollection services, bool enabled)
    {
        var assembly = AssemblyReference.Assembly;

        services.AddMvcCore().ConfigureApplicationPartManager(manager =>
        {
            var existing = manager.ApplicationParts
                .OfType<AssemblyPart>()
                .Where(p => p.Assembly == assembly)
                .ToList();

            if (enabled && existing.Count == 0)
                manager.ApplicationParts.Add(new AssemblyPart(assembly));

            if (!enabled)
                existing.ForEach(p => manager.ApplicationParts.Remove(p));

            if (enabled && !manager.FeatureProviders.OfType<CoreIdentityControllerFeatureProvider>().Any())
                manager.FeatureProviders.Add(new CoreIdentityControllerFeatureProvider());
        });
    }
}

/// Kayıtlı Identity context tipini taşır (migration/seed için).
public sealed record KaryaIdentityContextDescriptor(Type ContextType);
