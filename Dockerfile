# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project and solution files
COPY ChessBackende8/ChessBackende8.csproj ChessBackende8/
COPY ChessBackende8.sln ./

# Restore dependencies
RUN dotnet restore

# Copy the entire backend project
COPY ChessBackende8/. ChessBackende8/

# Set the working directory for the publish stage
WORKDIR /app/ChessBackende8

# Publish the application
RUN dotnet publish -c Release -o out

# Final stage: use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/ChessBackende8/out ./

# Health check (optional)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 CMD curl -f http://localhost:80/ || exit 1

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ChessBackende8.dll"]
