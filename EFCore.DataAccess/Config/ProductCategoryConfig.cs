using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ProductCategoryId)
                    .HasComment("Primary key for ProductCategory records.")
                    .HasColumnName("ProductCategoryID");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Category description.");
            modelBuilder.Property(e => e.ParentProductCategoryId)
                    .HasComment("Product category identification number of immediate ancestor category. Foreign key to ProductCategory.ProductCategoryID.")
                    .HasColumnName("ParentProductCategoryID");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");

            modelBuilder.HasOne(d => d.ParentProductCategory).WithMany(p => p.InverseParentProductCategory)
                    .HasForeignKey(d => d.ParentProductCategoryId)
                    .HasConstraintName("FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");

            // primary keys & name of table
            modelBuilder.HasKey(e => e.ProductCategoryId).HasName("PK_ProductCategory_ProductCategoryID");
            modelBuilder.ToTable("ProductCategory", "SalesLT", tb => tb.HasComment("High-level product categorization."));


            // index 
            modelBuilder.HasIndex(e => e.Name, "AK_ProductCategory_Name").IsUnique();
            modelBuilder.HasIndex(e => e.Rowguid, "AK_ProductCategory_rowguid").IsUnique();           
           
        }
    }
}
