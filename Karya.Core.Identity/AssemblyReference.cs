using Karya.Core.Identity.Application.Seeders;
using Karya.Core.Identity.Extensions;
using Karya.Core.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Karya.Core.Identity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    [Obsolete("AddKaryaIdentity<TContext>(configuration, o => o.UseDbContext(...)) kullanın.")]
    public static void AddCoreIdentityRegistiration<TIdentityContext>(this IServiceCollection services, IConfiguration configuration, string defaultConnectionName)
        where TIdentityContext : AppDbContext
    {
        var connectionString = configuration.GetConnectionString(defaultConnectionName);
        services.AddKaryaIdentity<TIdentityContext>(configuration, o => o.UseDbContext(b => b.UseSqlServer(connectionString)));
    }

    [Obsolete("AddKaryaSeeder<TSeeder>() kullanın.")]
    public static IServiceCollection AddCoreSeeder<TSeeder>(this IServiceCollection services) where TSeeder : class, IDatabaseSeeder
        => services.AddKaryaSeeder<TSeeder>();

    [Obsolete("app.UseKaryaIdentity() veya MigrateKaryaIdentityAsync() kullanın.")]
    public static Task MigrateCoreDatabaseAsync<TContext>(this IServiceProvider serviceProvider) where TContext : DbContext
        => serviceProvider.MigrateKaryaIdentityAsync(typeof(TContext), applyMigrations: true, runSeeders: true);
}
