# Use the ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
# If you need HTTPS (ensure you have certs configured), uncomment and adjust:
# EXPOSE 8081

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["DotGPT.sln", "./"]
COPY ["src/Core/DotGPT.Core.csproj", "src/Core/"]
COPY ["src/Application/DotGPT.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/DotGPT.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Server/DotGPT.Server.csproj", "src/Server/"]
COPY ["src/Client/DotGPT.Client.csproj", "src/Client/"]

# Restore dependencies for all projects
RUN dotnet restore "DotGPT.sln"

# Copy the rest of the source code
COPY . .

# Build the Server project in Release configuration
WORKDIR "/src/src/Server"
RUN dotnet build "DotGPT.Server.csproj" -c Release -o /app/build

# Build the Client project (for WASM assets)
WORKDIR "/src/src/Client"
# Note: We build the client here too, but the primary build happens in its own Dockerfile for multi-stage builds.
# This ensures assets are available if needed directly by the server build process, though typically we copy from the client stage.
RUN dotnet build "DotGPT.Client.csproj" -c Release -o /app/build_client # Build client separately if needed by server build

# Publish the Server project
FROM build AS publish
WORKDIR "/src/src/Server"
RUN dotnet publish "DotGPT.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: Use the base runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy Blazor WASM static assets from the Client build output
# We assume the Client build output is in wwwroot within the Server publish directory
# If Client builds separately, adjust the COPY source accordingly.
# The current setup builds Client and Server together, placing Client's wwwroot in Server's publish output.

ENTRYPOINT ["dotnet", "DotGPT.Server.dll"]

# Set ASPNETCORE_URLS to listen on the exposed port
ENV ASPNETCORE_URLS=http://+:8080
# If using HTTPS: ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
