using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.DataAccess.Config
{
    public class CustomerAddressConfig : IEntityTypeConfiguration<CustomerAddress>
    {

        public void Configure(EntityTypeBuilder<CustomerAddress> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.CustomerId)
                    .HasComment("Primary key. Foreign key to Customer.CustomerID.")
                    .HasColumnName("CustomerID");
            modelBuilder.Property(e => e.AddressId)
                    .HasComment("Primary key. Foreign key to Address.AddressID.")
                    .HasColumnName("AddressID");
            modelBuilder.Property(e => e.AddressType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("The kind of Address. One of: Archive, Billing, Home, Main Office, Primary, Shipping");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");

            modelBuilder.HasOne(d => d.Address).WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            // primary keys & name of table
            modelBuilder.HasKey(e => new { e.CustomerId, e.AddressId }).HasName("PK_CustomerAddress_CustomerID_AddressID");
            modelBuilder.ToTable("CustomerAddress", "SalesLT", tb => tb.HasComment("Cross-reference table mapping customers to their address(es)."));

            // index   
            modelBuilder.HasIndex(e => e.Rowguid, "AK_CustomerAddress_rowguid").IsUnique();          

        }
    }
}
