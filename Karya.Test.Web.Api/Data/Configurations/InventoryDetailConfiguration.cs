using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karya.Test.Web.Api.Data.Configurations;

public class InventoryDetailConfiguration : IEntityTypeConfiguration<InventoryDetail>
{
    public void Configure(EntityTypeBuilder<InventoryDetail> builder)
    {
        builder.ToTable("InventoryDetails");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Note).IsRequired();
        builder.HasOne(x => x.Inventory).WithMany(x => x.InventoryDetails).HasForeignKey(x => x.InventoryId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.MainCategory).WithMany().HasForeignKey(x => x.MainCategoryId).OnDelete(DeleteBehavior.NoAction);
    }
}