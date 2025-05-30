using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataAccess.Config
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> modelBuilder)
        {
            // name of table
            modelBuilder.ToTable("Customer", "SalesLT", tb => tb.HasComment("Customer information."));

            // name of columns
            modelBuilder.Property(e => e.CustomerId)
        .HasComment("Primary key for Customer records.")
        .HasColumnName("CustomerID");
            modelBuilder.Property(e => e.CompanyName)
                    .HasMaxLength(128)
                    .HasComment("The customer's organization.");
            modelBuilder.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .HasComment("E-mail address for the person.");
            modelBuilder.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("First name of the person.");
            modelBuilder.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Last name of the person.");
            modelBuilder.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .HasComment("Middle name or middle initial of the person.");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.NameStyle).HasComment("0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.");
            modelBuilder.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasComment("Password for the e-mail account.");
            modelBuilder.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Random value concatenated with the password string before the password is hashed.");
            modelBuilder.Property(e => e.Phone)
                    .HasMaxLength(25)
                    .HasComment("Phone number associated with the person.");
            modelBuilder.Property(e => e.Rowguid)
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                    .HasColumnName("rowguid");
            modelBuilder.Property(e => e.SalesPerson)
                    .HasMaxLength(256)
                    .HasComment("The customer's sales person, an employee of AdventureWorks Cycles.");
            modelBuilder.Property(e => e.Suffix)
                    .HasMaxLength(10)
                    .HasComment("Surname suffix. For example, Sr. or Jr.");
            modelBuilder.Property(e => e.Title)
                    .HasMaxLength(8)
                    .HasComment("A courtesy title. For example, Mr. or Ms.");

            // primary key
            modelBuilder.HasKey(e => e.CustomerId).HasName("PK_Customer_CustomerID");

            // index
            modelBuilder.HasIndex(e => e.Rowguid, "AK_Customer_rowguid").IsUnique();
            modelBuilder.HasIndex(e => e.EmailAddress, "IX_Customer_EmailAddress");
        }
    }
}
