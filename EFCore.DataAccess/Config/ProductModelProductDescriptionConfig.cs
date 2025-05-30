using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class ProductModelProductDescriptionConfig : IEntityTypeConfiguration<ProductModelProductDescription>
    {

        public void Configure(EntityTypeBuilder<ProductModelProductDescription> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ProductModelId)
                  .HasComment("Primary key. Foreign key to ProductModel.ProductModelID.")
                  .HasColumnName("ProductModelID");
            modelBuilder.Property(e => e.ProductDescriptionId)
                    .HasComment("Primary key. Foreign key to ProductDescription.ProductDescriptionID.")
                    .HasColumnName("ProductDescriptionID");
            modelBuilder.Property(e => e.Culture)
                    .HasMaxLength(6)
                    .IsFixedLength()
                    .HasComment("The culture for which the description is written");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("rowguid");

            modelBuilder.HasOne(d => d.ProductDescription).WithMany(p => p.ProductModelProductDescriptions)
                    .HasForeignKey(d => d.ProductDescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasOne(d => d.ProductModel).WithMany(p => p.ProductModelProductDescriptions)
                    .HasForeignKey(d => d.ProductModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


            // primary keys & name of table
            modelBuilder.HasKey(e => new { e.ProductModelId, e.ProductDescriptionId, e.Culture }).HasName("PK_ProductModelProductDescription_ProductModelID_ProductDescriptionID_Culture");
            modelBuilder.ToTable("ProductModelProductDescription", "SalesLT", tb => tb.HasComment("Cross-reference table mapping product descriptions and the language the description is written in."));


            // index   
            modelBuilder.HasIndex(e => e.Rowguid, "AK_ProductModelProductDescription_rowguid").IsUnique();         
          
        }
    }
}
