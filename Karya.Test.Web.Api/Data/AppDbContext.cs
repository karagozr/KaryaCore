using Karya.Core.Indentity.Domains.Entities;
using Karya.Test.Web.Api.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

// Define a custom IdentityUser with Guid as the key


public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseOpenIddict();

        modelBuilder.Entity<LocalizationResource>(b =>
        {
            b.ToTable("LocalizationResources");
            b.HasKey(x => x.Id);
            b.Property(x => x.Code).IsRequired().HasMaxLength(150);
            b.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
            b.Property(x => x.Value).IsRequired();
            b.HasIndex(x => new { x.Code, x.LanguageCode }).IsUnique();
        });
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Persist Security Info=True;Data Source=.;Initial Catalog=DEV_TEST;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes");

    //    base.OnConfiguring(optionsBuilder);
    //}
}
