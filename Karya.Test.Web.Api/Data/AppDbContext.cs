using Karya.Test.Web.Api.Localization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class AppDbContext : Karya.Core.Indentity.Infrastructure.AppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
}
