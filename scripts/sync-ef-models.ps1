#Requires -Version 5.1
<#
.SYNOPSIS
  Regenerates EF Core entities and DbContext from the live SQL Server schema.

.DESCRIPTION
  Reads the connection string from EFCore-DataQuality/appsettings.json (or the
  ConnectionStrings__DefaultConnection environment variable), runs EF Core Power
  Tools CLI (efcpt), removes obsolete Config/*.cs files from the old layout,
  then builds and tests the solution.

.PARAMETER ConnectionName
  Connection string name in appsettings.json. Default: DefaultConnection.

.PARAMETER SkipTests
  Build only; do not run dotnet test.

.PARAMETER SkipBuild
  Regenerate only; do not build or test.

.EXAMPLE
  .\scripts\sync-ef-models.ps1

.EXAMPLE
  $env:ConnectionStrings__DefaultConnection = "Server=.;Database=AdventureWorksLT2016;..."
  .\scripts\sync-ef-models.ps1
#>
[CmdletBinding()]
param(
    [string] $ConnectionName = "DefaultConnection",
    [switch] $SkipTests,
    [switch] $SkipBuild
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$dataAccessDir = Join-Path $repoRoot "EFCore.DataAccess"
$testProjectDir = Join-Path $repoRoot "EFCore-DataQuality"
$appsettingsPath = Join-Path $testProjectDir "appsettings.json"
$configPath = Join-Path $dataAccessDir "efcpt-config.json"

function Get-ConnectionString {
    $fromEnv = [Environment]::GetEnvironmentVariable("ConnectionStrings__${ConnectionName}")
    if (-not [string]::IsNullOrWhiteSpace($fromEnv)) {
        return $fromEnv
    }

    if (-not (Test-Path $appsettingsPath)) {
        throw "appsettings.json not found at: $appsettingsPath"
    }

    $json = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
    $value = $json.ConnectionStrings.$ConnectionName
    if ([string]::IsNullOrWhiteSpace($value)) {
        throw "Connection string '$ConnectionName' not found in $appsettingsPath"
    }
    return $value
}

function Remove-ObsoleteGeneratedArtifacts {
    # Legacy Power Tools layout (IEntityTypeConfiguration per entity); efcpt uses inline fluent API.
    $configDir = Join-Path $dataAccessDir "Config"
    if (Test-Path $configDir) {
        Remove-Item $configDir -Recurse -Force
    }

    $readme = Join-Path $dataAccessDir "efcpt-readme.md"
    if (Test-Path $readme) {
        Remove-Item $readme -Force
    }
}

Write-Host "==> Restoring dotnet tools (efcpt)..." -ForegroundColor Cyan
Push-Location $repoRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw "dotnet tool restore failed." }

    $connectionString = Get-ConnectionString
    Write-Host "==> Reverse engineering database into EFCore.DataAccess..." -ForegroundColor Cyan

    Push-Location $dataAccessDir
    try {
        dotnet tool run efcpt -- $connectionString mssql -i $configPath -o .
        if ($LASTEXITCODE -ne 0) { throw "efcpt failed with exit code $LASTEXITCODE." }
    }
    finally {
        Pop-Location
    }

    Remove-ObsoleteGeneratedArtifacts

    if ($SkipBuild) {
        Write-Host "==> Skipping build and tests (SkipBuild)." -ForegroundColor Yellow
        return
    }

    Write-Host "==> Building solution..." -ForegroundColor Cyan
    Push-Location $testProjectDir
    try {
        dotnet build
        if ($LASTEXITCODE -ne 0) { throw "dotnet build failed." }

        if (-not $SkipTests) {
            Write-Host "==> Running tests..." -ForegroundColor Cyan
            dotnet test --no-build
            if ($LASTEXITCODE -ne 0) { throw "dotnet test failed." }
        }
    }
    finally {
        Pop-Location
    }

    Write-Host "==> EF model sync completed successfully." -ForegroundColor Green
}
finally {
    Pop-Location
}
