using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.DataAccess.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.Orderdesc)
                .HasMaxLength(10)
                .IsFixedLength();
            modelBuilder.Property(e => e.Ordername)
                    .HasMaxLength(10)
                    .IsFixedLength();

            // primary keys & name of table
            modelBuilder.HasNoKey()
                  .ToTable("Order");

            // index
        }
    }
}
