using Microsoft.EntityFrameworkCore;

namespace EFCore.DataAccess.Data;

/// <summary>
/// Hand-maintained partial: connection-string constructor for tests.
/// Preserved across efcpt regenerations (do not edit generated context files).
/// </summary>
public partial class AdventureWorksLT2016Context
{
    private readonly string? _connectionString;

    public AdventureWorksLT2016Context(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
