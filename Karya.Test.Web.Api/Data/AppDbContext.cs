using Karya.Core.Indentity.Domains.Entities;
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

        // Replace default OpenIddict entities with the App-prefixed ones.
        modelBuilder.UseOpenIddict<AppApplication, AppAuthorization, AppScope, AppToken, Guid>();

        modelBuilder.Entity<AppRoleGroup>(b =>
        {
            b.ToTable("AppRoleGroups");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.Property(x => x.TenantId).HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<AppRoleGroupRole>(b =>
        {
            b.ToTable("AppRoleGroupRoles");
            b.HasKey(x => new { x.RoleGroupId, x.RoleId });

            b.HasOne(x => x.RoleGroup)
                .WithMany(g => g.RoleGroupRoles)
                .HasForeignKey(x => x.RoleGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Role)
                .WithMany(r => r.RoleGroupRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppUserRoleGroup>(b =>
        {
            b.ToTable("AppUserRoleGroups");
            b.HasKey(x => new { x.UserId, x.RoleGroupId });

            b.HasOne(x => x.User)
                .WithMany(u => u.UserRoleGroups)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.RoleGroup)
                .WithMany(g => g.UserRoleGroups)
                .HasForeignKey(x => x.RoleGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

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
