# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the backend solution file
COPY ChessBackende8.sln ./

# Copy the project file for the backend
COPY ChessBackende8/ChessBackende8.csproj ChessBackende8/

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

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ChessBackende8.dll"]
