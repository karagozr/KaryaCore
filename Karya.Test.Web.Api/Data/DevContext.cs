using Karya.Core.Common.Attributes.Data;
using Karya.Test.Web.Api.Entities;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Karya.Test.Web.Api.Data;

public class DevContext:DbContext
{
    const string Connection1 = "Persist Security Info=True;Data Source=.;Initial Catalog=DEV_TEST;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes";
    const string Connection2 = "Persist Security Info=True;Data Source=OZFTYNBERP01\\SQLEXPRESS;Initial Catalog=DEV_TEST;User ID=sa;Password=Santral123*;Integrated Security=True;TrustServerCertificate=Yes";


    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryCategory> InventoryCategories => Set<InventoryCategory>();

    public DbSet<User> Users => Set<User>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Connection1);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var navigationProperties = entityType.ClrType.GetProperties()
                .Where(p => p.GetCustomAttribute<TenantForeignKeyAttribute>() != null);

            foreach (var navProp in navigationProperties)
            {
                var attr = navProp.GetCustomAttribute<TenantForeignKeyAttribute>()!;
                string categoryIdName = attr.CategoryIdPropertyName;
                string navigationName = navProp.Name; 

                modelBuilder.Entity(entityType.ClrType)
                    .HasOne(navProp.PropertyType, navigationName)
                    .WithMany()
                    .HasForeignKey(new[] { "TenantId", categoryIdName })
                    .HasPrincipalKey(new[] { "TenantId", "Id" })
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
