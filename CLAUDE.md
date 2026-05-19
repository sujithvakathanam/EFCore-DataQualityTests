# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Purpose

A C# solution demonstrating how Entity Framework Core can be used to implement data quality tests for an ETL project. Tests connect to an AdventureWorksLT2016 SQL Server database and run NUnit-based checks for row counts, duplicates, and null values.

## Solution Structure

Two projects in `EFCore-DataQuality/EFCore-DataQuality.sln`:

- **EFCore.DataAccess** — Data access layer. Contains the EF Core `DbContext` (`Data/AdventureWorksLT2016Context.cs`), auto-generated POCO entities (`Entities/`), and Fluent API configuration classes (`Config/`). Entities were reverse-engineered from the database using EF Core Power Tools.
- **EFCore-DataQuality** — NUnit test project. Contains data quality test fixtures (`SalesLT/`), utility/extension methods (`Utils/`), a `DBHelper.cs` for reading the connection string, and `appsettings.json`.

## Prerequisites

- .NET 9.0 SDK
- SQL Server (local instance) with the AdventureWorksLT2016 sample database
- Connection string configured in `EFCore-DataQuality/appsettings.json`

## Commands

```powershell
# Restore, build, and run all tests
dotnet restore
dotnet build
dotnet test

# Run a single test class (e.g., ProductTests)
dotnet test --filter "FullyQualifiedName~ProductTests"

# Run a single test method
dotnet test --filter "Name=NoDuplicateProductNumber"

# Run with detailed output
dotnet test --verbosity detailed --logger:"console;verbosity=detailed"
```

All commands should be run from the `EFCore-DataQuality/` directory (where the `.sln` file lives).

## Testing Patterns

Each test fixture follows this structure:
- `[SetUp]` calls `DBHelper.GetConnectionString()` to load the connection string from `appsettings.json`
- Each `[Test]` instantiates a new `AdventureWorksLT2016Context` via `new(connectionString)` inside a `using` block for disposal

Three recurring data quality check patterns:

1. **Row count** — `context.Products.GetRowCount() > 0` using the `GetRowCount<T>()` extension method
2. **Duplicate check** — LINQ `GroupBy()` on a key field; assert the duplicate count is 0
3. **Null/empty check** — `FirstOrDefault(x => x.Field == null || x.Field == "")` on a DbSet; assert the result is null (no bad rows found)

`Utils/Extensions.cs` provides `GetMoreThanOnceRepeated<T>()` (duplicates excluding first) and `GetAllDuplicates<T>()` for reuse across tests.

## Adding New Tests

1. Create a `[TestFixture]` class under `EFCore-DataQuality/SalesLT/` (or a new folder for a different schema).
2. Add a `string _connectionString` field, a `[SetUp]` method that calls `DBHelper.GetConnectionString()`, and `[Test]` methods using the patterns above.
3. Reference `EFCore.DataAccess` entities and DbContext via the existing project reference — no new NuGet packages needed for standard checks.
