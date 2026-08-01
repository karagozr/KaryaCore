using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Test.Web.Api.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

// Define a custom IdentityUser with Guid as the key


public class AppDbContext : IdentityDbContext<
    AppUser,
    AppRole,
    Guid,
    AppUserClaim,
    AppUserRole,
    AppUserLogin,
    AppRoleClaim,
    AppUserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();

    public DbSet<AppRoleGroup> RoleGroups => Set<AppRoleGroup>();

    public DbSet<AppRoleGroupRole> RoleGroupRoles => Set<AppRoleGroupRole>();

    public DbSet<AppUserRoleGroup> UserRoleGroups => Set<AppUserRoleGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // OpenIddict + rol grubu model konfigürasyonu Karya.Core.Identity içinde.
        modelBuilder.ApplyKaryaIdentityModel();

        modelBuilder.Entity<LocalizationResource>(b =>
        {
            b.ToTable("LocalizationResources");
            b.HasKey(x => x.Id);
            b.Property(x => x.Code).IsRequired().HasMaxLength(150);
            b.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
            b.Property(x => x.Value).IsRequired();
            b.Property(x => x.Scope).HasConversion<byte>();
            b.HasIndex(x => new { x.Code, x.LanguageCode, x.Scope }).IsUnique();
        });
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Persist Security Info=True;Data Source=.;Initial Catalog=DEV_TEST;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes");

    //    base.OnConfiguring(optionsBuilder);
    //}
}
