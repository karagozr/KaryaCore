using Karya.Test.Web.Api.Data.Configurations;
using Karya.Test.Web.Api.Localization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class AppDbContext : Karya.Core.Indentity.Infrastructure.AppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LocalizationResourceConfiguration());
    }
}
