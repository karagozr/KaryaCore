using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
namespace Karya.Core.Indentity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    public static void AddCoreIdentityRegistiration<TIdentityContext>(this IServiceCollection services) where TIdentityContext : Infrastructure.AppDbContext
    {
        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<TIdentityContext>();

        // Repository/UnitOfWork'ün kullandığı soyut DbContext, Identity context'ine yönlendirilir.
        services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(sp => sp.GetRequiredService<TIdentityContext>());

        // Yetki servisi (SystemAdmin/TenantAdmin) MediatR AuthorizationBehavior için.
        services.AddScoped<Karya.Core.App.Interfaces.Services.IPermissionService, Services.IdentityPermissionService>();

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
                options.SetTokenEndpointUris("/connect/token");

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
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
    }
}
