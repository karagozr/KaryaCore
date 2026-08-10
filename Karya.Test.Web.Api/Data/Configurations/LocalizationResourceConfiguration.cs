using Karya.Test.Web.Api.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karya.Test.Web.Api.Data.Configurations;

public class LocalizationResourceConfiguration
    : IEntityTypeConfiguration<LocalizationResource>
{
    public void Configure(
        EntityTypeBuilder<LocalizationResource> builder)
    {
        builder.ToTable("LocalizationResources");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(150);
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Scope).HasConversion<byte>();
        builder.HasIndex(x => new
        {
            x.Code,
            x.LanguageCode,
            x.Scope
        }).IsUnique();
    }
}