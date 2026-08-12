using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karya.Test.Web.Api.Data.Configurations;

public class InventoryMainCategoryConfiguration
    : IEntityTypeConfiguration<InventoryMainCategory>
{
    public void Configure(
        EntityTypeBuilder<InventoryMainCategory> builder)
    {
        builder.ToTable("InventoryMainCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
    }
}