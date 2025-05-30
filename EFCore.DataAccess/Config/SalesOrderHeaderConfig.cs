using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class SalesOrderHeaderConfig : IEntityTypeConfiguration<SalesOrderHeader>
    {    

        public void Configure(EntityTypeBuilder<SalesOrderHeader> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.SalesOrderId)
                    .HasComment("Primary key.")
                    .HasColumnName("SalesOrderID");
            modelBuilder.Property(e => e.AccountNumber)
                    .HasMaxLength(15)
                    .HasComment("Financial accounting number reference.");
            modelBuilder.Property(e => e.BillToAddressId)
                    .HasComment("The ID of the location to send invoices.  Foreign key to the Address table.")
                    .HasColumnName("BillToAddressID");
            modelBuilder.Property(e => e.Comment).HasComment("Sales representative comments.");
            modelBuilder.Property(e => e.CreditCardApprovalCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("Approval code provided by the credit card company.");
            modelBuilder.Property(e => e.CustomerId)
                    .HasComment("Customer identification number. Foreign key to Customer.CustomerID.")
                    .HasColumnName("CustomerID");
            modelBuilder.Property(e => e.DueDate)
                    .HasComment("Date the order is due to the customer.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Freight)
                    .HasComment("Shipping cost.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.OnlineOrderFlag)
                    .HasDefaultValue(true)
                    .HasComment("0 = Order placed by sales person. 1 = Order placed online by customer.");
            modelBuilder.Property(e => e.OrderDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Dates the sales order was created.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.PurchaseOrderNumber)
                    .HasMaxLength(25)
                    .HasComment("Customer purchase order number reference. ");
            modelBuilder.Property(e => e.RevisionNumber).HasComment("Incremental number to track changes to the sales order over time.");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");
            modelBuilder.Property(e => e.SalesOrderNumber)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComputedColumnSql("(isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***'))", false)
                    .HasComment("Unique sales order identification number.");
            modelBuilder.Property(e => e.ShipDate)
                    .HasComment("Date the order was shipped to the customer.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.ShipMethod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Shipping method. Foreign key to ShipMethod.ShipMethodID.");
            modelBuilder.Property(e => e.ShipToAddressId)
                    .HasComment("The ID of the location to send goods.  Foreign key to the Address table.")
                    .HasColumnName("ShipToAddressID");
            modelBuilder.Property(e => e.Status)
                    .HasDefaultValue((byte)1)
                    .HasComment("Order current status. 1 = In process; 2 = Approved; 3 = Backordered; 4 = Rejected; 5 = Shipped; 6 = Cancelled");
            modelBuilder.Property(e => e.SubTotal)
                    .HasComment("Sales subtotal. Computed as SUM(SalesOrderDetail.LineTotal)for the appropriate SalesOrderID.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.TaxAmt)
                    .HasComment("Tax amount.")
                    .HasColumnType("money");
            modelBuilder.Property(e => e.TotalDue)
                    .HasComputedColumnSql("(isnull(([SubTotal]+[TaxAmt])+[Freight],(0)))", false)
                    .HasComment("Total due from customer. Computed as Subtotal + TaxAmt + Freight.")
                    .HasColumnType("money");

            modelBuilder.HasOne(d => d.BillToAddress).WithMany(p => p.SalesOrderHeaderBillToAddresses)
                    .HasForeignKey(d => d.BillToAddressId)
                    .HasConstraintName("FK_SalesOrderHeader_Address_BillTo_AddressID");

            modelBuilder.HasOne(d => d.Customer).WithMany(p => p.SalesOrderHeaders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasOne(d => d.ShipToAddress).WithMany(p => p.SalesOrderHeaderShipToAddresses)
                    .HasForeignKey(d => d.ShipToAddressId)
                    .HasConstraintName("FK_SalesOrderHeader_Address_ShipTo_AddressID");


            // primary keys & name of table
            modelBuilder.HasKey(e => e.SalesOrderId).HasName("PK_SalesOrderHeader_SalesOrderID");

            modelBuilder.ToTable("SalesOrderHeader", "SalesLT", tb =>
            {
                tb.HasComment("General sales order information.");
                tb.HasTrigger("uSalesOrderHeader");
            });

            // index 
            modelBuilder.HasIndex(e => e.SalesOrderNumber, "AK_SalesOrderHeader_SalesOrderNumber").IsUnique();

            modelBuilder.HasIndex(e => e.Rowguid, "AK_SalesOrderHeader_rowguid").IsUnique();

            modelBuilder.HasIndex(e => e.CustomerId, "IX_SalesOrderHeader_CustomerID");            
        }
    }
}
