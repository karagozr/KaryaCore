using Karya.Core.Identity.Application.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Karya.Core.Identity.Extensions;

public static class CoreIdentityApplicationBuilderExtensions
{

    public static IApplicationBuilder UseKaryaIdentity(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<CoreIdentityOptions>>().Value;

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
