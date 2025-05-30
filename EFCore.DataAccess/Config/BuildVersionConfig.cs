using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.DataAccess.Config
{
    public class BuildVersionConfig : IEntityTypeConfiguration<BuildVersion>
    {
        public void Configure(EntityTypeBuilder<BuildVersion> modelBuilder)
        {
             // name of columns
            modelBuilder.Property(e => e.DatabaseVersion)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasComment("Version number of the database in 9.yy.mm.dd.00 format.")
                    .HasColumnName("Database Version");
            modelBuilder.Property(e => e.ModifiedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.SystemInformationId)
                    .ValueGeneratedOnAdd()
                    .HasComment("Primary key for BuildVersion records.")
                    .HasColumnName("SystemInformationID");
            modelBuilder.Property(e => e.VersionDate)
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");

            // primary keys & name of table
            modelBuilder
                .HasNoKey()
                .ToTable("BuildVersion", tb => tb.HasComment("Current version number of the AdventureWorksLT 2012 sample database. "));

            // index       

        }
    }
}
