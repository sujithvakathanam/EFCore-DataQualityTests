# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory inside container
WORKDIR /app

# Copy all project files
COPY . ./ 

WORKDIR /app/EFCore-DataQuality

# Restore dependencies
RUN dotnet restore

# Build the solution in Release mode
RUN dotnet build --configuration Release

# Run tests
RUN dotnet test --no-build --verbosity normal --logger:"console;verbosity=detailed"
