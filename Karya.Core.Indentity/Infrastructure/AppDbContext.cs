using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

public abstract class AppDbContext : IdentityDbContext<
    AppUser,
    AppRole,
    Guid,
    AppUserClaim,
    AppUserRole,
    AppUserLogin,
    AppRoleClaim,
    AppUserToken>
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppRoleGroup> RoleGroups => Set<AppRoleGroup>();
    public DbSet<AppRoleGroupRole> RoleGroupRoles => Set<AppRoleGroupRole>();
    public DbSet<AppUserRoleGroup> UserRoleGroups => Set<AppUserRoleGroup>();
    public DbSet<AppUserTenant> UserTenants => Set<AppUserTenant>();
    public DbSet<AppTenant> Tenants => Set<AppTenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyKaryaIdentityModel();
    }
}
