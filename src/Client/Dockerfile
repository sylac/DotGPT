# Stage 1: Build the Blazor WebAssembly application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Client/DotGPT.Client.csproj", "src/Client/"]
COPY ["src/Core/DotGPT.Core.csproj", "src/Core/"]

# Restore dependencies for the Client project
WORKDIR /src/src/Client
RUN dotnet restore "DotGPT.Client.csproj"

# Copy the entire Client source code and potentially shared projects
WORKDIR /src
COPY ["src/Client/", "src/Client/"]
COPY ["src/Core/", "src/Core/"]

# Publish the Client project
WORKDIR /src/src/Client
RUN dotnet publish "./DotGPT.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Serve the static files using Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the published Blazor WASM files from the build stage
COPY --from=build /app/publish/wwwroot .

# Copy the custom Nginx configuration file
# Ensure nginx.conf exists in src/Client/
COPY src/Client/nginx.conf /etc/nginx/nginx.conf

# Expose port 80 for Nginx
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
