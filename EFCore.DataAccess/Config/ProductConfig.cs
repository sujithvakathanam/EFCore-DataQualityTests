using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.DataAccess.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {     

        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ProductId)
        .HasComment("Primary key for Product records.")
        .HasColumnName("ProductID");
            modelBuilder.Property(e => e.Color)
                    .HasMaxLength(15)
                    .HasComment("Product color.");
            modelBuilder.Property(e => e.DiscontinuedDate)
                    .HasComment("Date the product was discontinued.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.ListPrice)
                    .HasComment("Selling price.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of the product.");
            modelBuilder.Property(e => e.ProductCategoryId)
                    .HasComment("Product is a member of this product category. Foreign key to ProductCategory.ProductCategoryID. ")
                    .HasColumnName("ProductCategoryID");
            modelBuilder.Property(e => e.ProductModelId)
                    .HasComment("Product is a member of this product model. Foreign key to ProductModel.ProductModelID.")
                    .HasColumnName("ProductModelID");
            modelBuilder.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("Unique product identification number.");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");
            modelBuilder.Property(e => e.SellEndDate)
                    .HasComment("Date the product was no longer available for sale.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.SellStartDate)
                    .HasComment("Date the product was available for sale.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Size)
                    .HasMaxLength(5)
                    .HasComment("Product size.");
            modelBuilder.Property(e => e.StandardCost)
                    .HasComment("Standard cost of the product.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.ThumbNailPhoto).HasComment("Small image of the product.");
            modelBuilder.Property(e => e.ThumbnailPhotoFileName)
                    .HasMaxLength(50)
                    .HasComment("Small image file name.");
            modelBuilder.Property(e => e.Weight)
                    .HasComment("Product weight.")
                    .HasColumnType("decimal(8, 2)");
            modelBuilder.HasOne(d => d.ProductCategory).WithMany(p => p.Products).HasForeignKey(d => d.ProductCategoryId);
            modelBuilder.HasOne(d => d.ProductModel).WithMany(p => p.Products).HasForeignKey(d => d.ProductModelId);


            // primary keys & name of table
            modelBuilder.HasKey(e => e.ProductId).HasName("PK_Product_ProductID");
            modelBuilder.ToTable("Product", "SalesLT", tb => tb.HasComment("Products sold or used in the manfacturing of sold products."));


            // index 
            modelBuilder.HasIndex(e => e.Name, "AK_Product_Name").IsUnique();

            modelBuilder.HasIndex(e => e.ProductNumber, "AK_Product_ProductNumber").IsUnique();

            modelBuilder.HasIndex(e => e.Rowguid, "AK_Product_rowguid").IsUnique();


         

        }
    }
}
