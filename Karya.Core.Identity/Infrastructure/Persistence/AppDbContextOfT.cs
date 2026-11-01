using Karya.Core.Identity.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence;

/// <summary>
/// Genişletilmiş kullanıcı/rol entity'leri ile çalışan Identity context'i.
/// <typeparamref name="TUser"/> / <typeparamref name="TRole"/> varsayılandan farklıysa
/// AspNetUsers / AspNetRoles tablolarında TPH (Discriminator) ile eşlenir.
/// </summary>
public abstract class AppDbContext<TUser, TRole> : AppDbContext
    where TUser : AppUser
    where TRole : AppRole
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TUser> AppUsers => Set<TUser>();
    public DbSet<TRole> AppRoles => Set<TRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (typeof(TUser) != typeof(AppUser))
            modelBuilder.Entity<TUser>();

        if (typeof(TRole) != typeof(AppRole))
            modelBuilder.Entity<TRole>();
    }
}
