using Karya.Test.Web.Api.Entities;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

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
        optionsBuilder.UseSqlServer(Connection2);
    }
}
