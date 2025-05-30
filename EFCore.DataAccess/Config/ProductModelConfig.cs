using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class ProductModelConfig : IEntityTypeConfiguration<ProductModel>
    {   
        public void Configure(EntityTypeBuilder<ProductModel> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ProductModelId).HasColumnName("ProductModelID");
            modelBuilder.Property(e => e.CatalogDescription).HasColumnType("xml");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("rowguid");

            // primary keys & name of table
            modelBuilder.HasKey(e => e.ProductModelId).HasName("PK_ProductModel_ProductModelID");

            modelBuilder.ToTable("ProductModel", "SalesLT");

            // index    
            modelBuilder.HasIndex(e => e.Name, "AK_ProductModel_Name").IsUnique();

            modelBuilder.HasIndex(e => e.Rowguid, "AK_ProductModel_rowguid").IsUnique();

            modelBuilder.HasIndex(e => e.CatalogDescription, "PXML_ProductModel_CatalogDescription");
        }
    }
}
