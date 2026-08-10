using Karya.Test.Web.Api.Entities;
using Karya.Test.Web.Api.Localization;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class AppDbContext : Karya.Core.Indentity.Infrastructure.AppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryDetail> InventoryDetails => Set<InventoryDetail>();
    public DbSet<InventoryCategory> InventoryCategories => Set<InventoryCategory>();
    public DbSet<InventoryMainCategory> InventoryMainCategories => Set<InventoryMainCategory>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
