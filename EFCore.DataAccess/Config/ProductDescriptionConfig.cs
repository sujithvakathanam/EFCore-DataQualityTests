using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class ProductDescriptionConfig : IEntityTypeConfiguration<ProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductDescription> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ProductDescriptionId)
                  .HasComment("Primary key for ProductDescription records.")
                  .HasColumnName("ProductDescriptionID");
            modelBuilder.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasComment("Description of the product.");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");

            // primary keys & name of table
            modelBuilder.HasKey(e => e.ProductDescriptionId).HasName("PK_ProductDescription_ProductDescriptionID");
            modelBuilder.ToTable("ProductDescription", "SalesLT", tb => tb.HasComment("Product descriptions in several languages."));


            // index
            modelBuilder.HasIndex(e => e.Rowguid, "AK_ProductDescription_rowguid").IsUnique();

        }
    }
}
