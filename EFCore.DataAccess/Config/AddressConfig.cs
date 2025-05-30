using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> modelBuilder)
        {
            // name of table
            modelBuilder.ToTable("Address", "SalesLT", tb => tb.HasComment("Street address information for customers."));

            // name of columns
            modelBuilder.Property(e => e.AddressId)
                    .HasComment("Primary key for Address records.")
                    .HasColumnName("AddressID");
            modelBuilder.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasComment("First street address line.");
            modelBuilder.Property(e => e.AddressLine2)
                    .HasMaxLength(60)
                    .HasComment("Second street address line.");
            modelBuilder.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Name of the city.");
            modelBuilder.Property(e => e.CountryRegion)
                    .IsRequired()
                    .HasMaxLength(50);
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasComment("Postal code for the street address.");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");
            modelBuilder.Property(e => e.StateProvince)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of state or province.");

            // primary key
            modelBuilder.HasKey(e => e.AddressId).HasName("PK_Address_AddressID");

            // index
            modelBuilder.HasIndex(e => e.Rowguid, "AK_Address_rowguid").IsUnique();

            modelBuilder.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.StateProvince, e.PostalCode, e.CountryRegion }, "IX_Address_AddressLine1_AddressLine2_City_StateProvince_PostalCode_CountryRegion");

            modelBuilder.HasIndex(e => e.StateProvince, "IX_Address_StateProvince");
        }
    }
}
