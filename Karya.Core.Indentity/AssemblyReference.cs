using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Indentity.Seeders;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
namespace Karya.Core.Indentity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    public static void AddCoreIdentityRegistiration<TIdentityContext>(this IServiceCollection services, IConfiguration configuration, string defaultConnectionName) where TIdentityContext 
        : DbContext
    {
        var connectionString = configuration.GetConnectionString(defaultConnectionName);
        services.AddDbContext<TIdentityContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<TIdentityContext>();

        // Repository/UnitOfWork'ün kullandığı soyut DbContext, Identity context'ine yönlendirilir.
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TIdentityContext>());

        services.AddScoped<AppUserTenantService>();
        services.AddScoped<AppRoleService>();
        services.AddScoped<AppUserRoleService>();
        services.AddScoped<AppUserRoleGroupService>();
        services.AddScoped<AppRoleGroupRoleService>();
        services.AddScoped<AppRoleClaimService>();

        // Yetki servisi (SystemAdmin/TenantAdmin) MediatR AuthorizationBehavior için.
        services.AddScoped<Karya.Core.App.Interfaces.Services.IPermissionService, Services.IdentityPermissionService>();

        services.AddHttpContextAccessor();
        services.AddScoped<AppAuthService>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IDatabaseSeeder, IdentityDataSeeder>();
        services.AddScoped<IDatabaseSeeder, PermissionSeeder>();

        // OpenIddict kaydı (App-prefixed entity'ler ile)
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<TIdentityContext>()
                    .ReplaceDefaultEntities<AppApplication, AppAuthorization, AppScope, AppToken, Guid>();
                //options.UseQuartz(); // Token temizliği için arka plan servisi
            })
            .AddServer(options =>
            {
                // Endpoint tanımları
                options.SetTokenEndpointUris("/connect/token", "/api/auth/login");

                // Akış (Flow) izinleri
                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                // Sertifikalar (Production'da gerçek sertifika kullanılmalı)
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Refresh Token Ayarları
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));
                options.AcceptAnonymousClients(); // Client_id zorunluluğu durumuna göre

                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();

                options.RemoveEventHandler(
                        OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreHandlers
                            .ExtractPostRequest<OpenIddictServerEvents.ExtractTokenRequestContext>.Descriptor);

                options.AddEventHandler<OpenIddictServerEvents.ExtractTokenRequestContext>(builder =>
                {
                    builder.UseScopedHandler<ExtractJsonTokenRequestHandler>();

                    builder.SetOrder(
                        OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreHandlers
                            .ExtractPostRequest<OpenIddictServerEvents.ExtractTokenRequestContext>.Descriptor.Order);
                });
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
    }

    public static IServiceCollection AddCoreSeeder<TSeeder>(this IServiceCollection services) where TSeeder : class, IDatabaseSeeder
    {
        services.AddScoped<IDatabaseSeeder, TSeeder>();

        return services;
    }

    public static async Task MigrateCoreDatabaseAsync<TContext>(this IServiceProvider serviceProvider) where TContext : DbContext
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<TContext>();

        await dbContext.Database.MigrateAsync();

        var seeders = scope.ServiceProvider
            .GetServices<IDatabaseSeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}
