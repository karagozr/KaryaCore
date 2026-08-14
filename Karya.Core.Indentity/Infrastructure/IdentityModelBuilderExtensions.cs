using Karya.Core.Indentity.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>
/// Model configuration for the Karya identity domain. Encapsulates the
/// OpenIddict entity replacement and role group mappings so that consuming
/// DbContexts do not need to reference OpenIddict directly.
/// </summary>
public static class IdentityModelBuilderExtensions
{
    public static ModelBuilder ApplyKaryaIdentityModel(this ModelBuilder modelBuilder)
    {
        // Replace default OpenIddict entities with the App-prefixed ones.
        modelBuilder.UseOpenIddict<AppApplication, AppAuthorization, AppScope, AppToken, Guid>();

        modelBuilder.Entity<AppRoleGroup>(b =>
        {
            b.ToTable("AppRoleGroups");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.Property(x => x.TenantId).IsRequired().HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<AppRoleGroupRole>(b =>
        {
            b.ToTable("AppRoleGroupRoles");
            b.HasKey(x => x.Id);
            b.Property(x => x.TenantId).IsRequired().HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.RoleGroupId, x.RoleId }).IsUnique();

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
            b.HasKey(x => x.Id);
            b.Property(x => x.TenantId).IsRequired().HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.UserId, x.RoleGroupId }).IsUnique();

            b.HasOne(x => x.User)
                .WithMany(u => u.UserRoleGroups)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.RoleGroup)
                .WithMany(g => g.UserRoleGroups)
                .HasForeignKey(x => x.RoleGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppUserTenant>(b =>
        {
            b.ToTable("AppUserTenants");
            b.HasKey(x => new { x.UserId, x.TenantId });
            b.Property(x => x.TenantId).IsRequired().HasMaxLength(256);

            b.HasOne(x => x.User)
                .WithMany(u => u.TenantMemberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.TenantId);
        });

        modelBuilder.Entity<AppTenant>(b =>
        {
            b.ToTable("AppTenants");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(256);
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
        });

        modelBuilder.Entity<AppUserClaim>(b =>
        {
            b.HasOne<AppUser>()
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<AppUserRole>(b =>
        {
            b.ToTable("AspNetUserRoles");
            b.HasKey(x => x.Id);
            b.Property(x => x.TenantId).IsRequired().HasMaxLength(256);
            b.HasIndex(x => new { x.TenantId, x.UserId, x.RoleId }).IsUnique();

            b.HasOne<AppUser>()
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne<AppRole>()
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<AppUserLogin>(b =>
        {
            b.HasOne<AppUser>()
                .WithMany(x => x.Logins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<AppUserToken>(b =>
        {
            b.HasOne<AppUser>()
                .WithMany(x => x.Tokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<AppRoleClaim>(b =>
        {
            b.HasOne<AppRole>()
                .WithMany(x => x.RoleClaims)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        return modelBuilder;
    }
}
