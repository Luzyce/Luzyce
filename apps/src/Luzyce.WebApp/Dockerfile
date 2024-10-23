# Use the .NET SDK to build the application as before
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Luzyce.Shared project
COPY ["../../../shared/src/Luzyce.Shared/Luzyce.Shared.csproj", "../../../shared/src/Luzyce.Shared/"]
RUN dotnet restore "../../../shared/src/Luzyce.Shared/Luzyce.Shared.csproj"
COPY ["../../../shared/src/Luzyce.Shared/", "../../../shared/src/Luzyce.Shared/"]

# Copy and build Luzyce.WebApp
COPY ["Luzyce.WebApp.csproj", "./"]
RUN dotnet restore "./Luzyce.WebApp.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./Luzyce.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./Luzyce.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the Nginx image to serve the static content
FROM nginx:alpine as final
WORKDIR /var/www/html

# Copy the published content from the publish stage to the nginx html directory
COPY --from=publish /app/publish/wwwroot .

# Setup nginx configuration
COPY apps/src/Luzyce.WebApp/nginx.conf /etc/nginx/nginx.conf

# Expose port 35133 for external access
EXPOSE 35133
