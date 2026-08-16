using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class DevContext : DbContext
{
    const string Connection1 = "Persist Security Info=True;Data Source=.;Initial Catalog=KARYA_WAPI_TEST_V1;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes";
    const string Connection2 = "Persist Security Info=True;Data Source=localhost\\SQLEXPRESS;Initial Catalog=DEV_TEST_PROC;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes";

    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryCategory> InventoryCategories => Set<InventoryCategory>();
    public DbSet<InventoryDetail> InventoryDetails => Set<InventoryDetail>();
    public DbSet<InventoryMainCategory> InventoryMainCategories => Set<InventoryMainCategory>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Connection1);
    }
}

