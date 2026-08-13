using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karya.Test.Web.Api.Data.Configurations;

public class InventoryCategoryConfiguration : IEntityTypeConfiguration<InventoryCategory>
{
    public void Configure(EntityTypeBuilder<InventoryCategory> builder)
    {
        builder.ToTable("InventoryCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.MainCategory).WithMany().HasForeignKey(x => x.MainCategoryId).OnDelete(DeleteBehavior.NoAction);
    }
}