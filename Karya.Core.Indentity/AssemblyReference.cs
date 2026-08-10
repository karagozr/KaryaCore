using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Karya.Core.Indentity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    public static void AddCoreIdentityRegistiration<TIdentityContext>(this IServiceCollection services, IConfiguration configuration) where TIdentityContext : Infrastructure.AppDbContext
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TIdentityContext>(options => options.UseSqlServer(connectionString));

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

    public static IServiceCollection AddKaryaSeeder<TSeeder>(this IServiceCollection services) where TSeeder : class, IDatabaseSeeder
    {
        services.AddScoped<IDatabaseSeeder, TSeeder>();

        return services;
    }

    public static async Task MigrateKaryaDatabaseAsync<TContext>(this IServiceProvider serviceProvider) where TContext : Infrastructure.AppDbContext
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
