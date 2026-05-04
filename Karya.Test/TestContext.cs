using Karya.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test;

public class TestContext:DbContext
{
    public DbSet<UserTest> TestItems => Set<UserTest>();
    public DbSet<UserTestLog> TestLogs => Set<UserTestLog>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Persist Security Info=True;Data Source=.;Initial Catalog=TestKarya;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes");
    }
}
