using Karya.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;


namespace Karya.Core.Abstracts.Contexts;

public abstract class BaseDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureTenantForeignKeys();
        base.OnModelCreating(modelBuilder);
    }
}
