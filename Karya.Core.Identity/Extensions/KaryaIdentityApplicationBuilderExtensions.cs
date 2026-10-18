using Karya.Core.Identity.Application.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Karya.Core.Identity.Extensions;

public static class KaryaIdentityApplicationBuilderExtensions
{
    /// <summary>
    /// Authentication/Authorization middleware'lerini ekler. Ayarlara göre başlangıçta
    /// migration uygular ve seeder'ları çalıştırır.
    /// </summary>
    public static IApplicationBuilder UseKaryaIdentity(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<KaryaIdentityOptions>>().Value;

        if (options.ApplyMigrationsOnStartup || options.RunSeedersOnStartup)
        {
            var descriptor = app.ApplicationServices.GetRequiredService<KaryaIdentityContextDescriptor>();

            app.ApplicationServices
                .MigrateKaryaIdentityAsync(descriptor.ContextType, options.ApplyMigrationsOnStartup, options.RunSeedersOnStartup)
                .GetAwaiter()
                .GetResult();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    /// <summary>Identity context migration'larını uygular ve kayıtlı seeder'ları çalıştırır.</summary>
    public static async Task MigrateKaryaIdentityAsync(this IServiceProvider serviceProvider, bool applyMigrations = true, bool runSeeders = true)
    {
        var descriptor = serviceProvider.GetRequiredService<KaryaIdentityContextDescriptor>();
        await serviceProvider.MigrateKaryaIdentityAsync(descriptor.ContextType, applyMigrations, runSeeders);
    }

    internal static async Task MigrateKaryaIdentityAsync(this IServiceProvider serviceProvider, Type contextType, bool applyMigrations, bool runSeeders)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        if (applyMigrations)
        {
            var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(contextType);
            await dbContext.Database.MigrateAsync();
        }

        if (!runSeeders)
            return;

        foreach (var seeder in scope.ServiceProvider.GetServices<IDatabaseSeeder>())
            await seeder.SeedAsync();
    }
}
