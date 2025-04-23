# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Server/DotGPT.Server.csproj", "src/Server/"]
COPY ["src/Infrastructure/DotGPT.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/DotGPT.Application.csproj", "src/Application/"]
COPY ["src/Core/DotGPT.Core.csproj", "src/Core/"]
RUN dotnet restore "./src/Server/DotGPT.Server.csproj"
COPY . .

WORKDIR "/src/src/Server"
RUN dotnet build "./DotGPT.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DotGPT.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the environment variable to use port 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "DotGPT.Server.dll"]