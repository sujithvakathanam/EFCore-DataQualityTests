using EFCore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCore.DataAccess.Config
{
    public class ErrorLogConfig : IEntityTypeConfiguration<ErrorLog>
    {

        public void Configure(EntityTypeBuilder<ErrorLog> modelBuilder)
        {
            // name of columns
            modelBuilder.Property(e => e.ErrorLogId)
                    .HasComment("Primary key for ErrorLog records.")
                    .HasColumnName("ErrorLogID");
            modelBuilder.Property(e => e.ErrorLine).HasComment("The line number at which the error occurred.");
            modelBuilder.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasComment("The message text of the error that occurred.");
            modelBuilder.Property(e => e.ErrorNumber).HasComment("The error number of the error that occurred.");
            modelBuilder.Property(e => e.ErrorProcedure)
                    .HasMaxLength(126)
                    .HasComment("The name of the stored procedure or trigger where the error occurred.");
            modelBuilder.Property(e => e.ErrorSeverity).HasComment("The severity of the error that occurred.");
            modelBuilder.Property(e => e.ErrorState).HasComment("The state number of the error that occurred.");
            modelBuilder.Property(e => e.ErrorTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("The date and time at which the error occurred.")
                    .HasColumnType("datetime");
            modelBuilder.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasComment("The user who executed the batch in which the error occurred.");

            // primary keys & name of table
            modelBuilder.HasKey(e => e.ErrorLogId).HasName("PK_ErrorLog_ErrorLogID");
            modelBuilder.ToTable("ErrorLog", tb => tb.HasComment("Audit table tracking errors in the the AdventureWorks database that are caught by the CATCH block of a TRY...CATCH construct. Data is inserted by stored procedure dbo.uspLogError when it is executed from inside the CATCH block of a TRY...CATCH construct."));

            // index  
        }
    }
}
