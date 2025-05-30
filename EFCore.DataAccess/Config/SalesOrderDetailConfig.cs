using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.DataAccess.Config
{
    public class SalesOrderDetailConfig : IEntityTypeConfiguration<SalesOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SalesOrderDetail> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.SalesOrderId)
                     .HasComment("Primary key. Foreign key to SalesOrderHeader.SalesOrderID.")
                     .HasColumnName("SalesOrderID");
            modelBuilder.Property(e => e.SalesOrderDetailId)
                    .ValueGeneratedOnAdd()
                    .HasComment("Primary key. One incremental unique number per product sold.")
                    .HasColumnName("SalesOrderDetailID");
            modelBuilder.Property(e => e.LineTotal)
                    .HasComputedColumnSql("(isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0)))", false)
                    .HasComment("Per product subtotal. Computed as UnitPrice * (1 - UnitPriceDiscount) * OrderQty.")
                    .HasColumnType("numeric(38, 6)");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.OrderQty).HasComment("Quantity ordered per product.");
            modelBuilder.Property(e => e.ProductId)
                    .HasComment("Product sold to customer. Foreign key to Product.ProductID.")
                    .HasColumnName("ProductID");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");
            modelBuilder.Property(e => e.UnitPrice)
                    .HasComment("Selling price of a single product.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.UnitPriceDiscount)
                    .HasComment("Discount amount.")
                    .HasColumnType("money");
            modelBuilder.HasOne(d => d.Product).WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderDetails).HasForeignKey(d => d.SalesOrderId);


            // primary keys & name of table
            modelBuilder.HasKey(e => new { e.SalesOrderId, e.SalesOrderDetailId }).HasName("PK_SalesOrderDetail_SalesOrderID_SalesOrderDetailID");

            modelBuilder.ToTable("SalesOrderDetail", "SalesLT", tb =>
            {
                tb.HasComment("Individual products associated with a specific sales order. See SalesOrderHeader.");
                tb.HasTrigger("iduSalesOrderDetail");
            });

            // index
            modelBuilder.HasIndex(e => e.Rowguid, "AK_SalesOrderDetail_rowguid").IsUnique();
            modelBuilder.HasIndex(e => e.ProductId, "IX_SalesOrderDetail_ProductID");
            
        }
    }
}
