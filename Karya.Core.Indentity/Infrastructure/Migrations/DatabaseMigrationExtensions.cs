using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Karya.Core.Indentity.Infrastructure.Migrations;

public static class DatabaseMigrationExtensions
{
    public static IServiceCollection AddKaryaSeeder<TSeeder>(this IServiceCollection services) where TSeeder : class, IDatabaseSeeder
    {
        services.AddScoped<IDatabaseSeeder, TSeeder>();
        return services;
    }

    public static async Task MigrateKaryaDatabaseAsync<TContext>(
        this IServiceProvider serviceProvider) where TContext : AppDbContext
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();

        var seeders = scope.ServiceProvider.GetServices<IDatabaseSeeder>();
        foreach (var seeder in seeders)
            await seeder.SeedAsync();
    }
}
