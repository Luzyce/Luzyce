# Use the .NET SDK to build the application as before
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["LuzyceWebApp/LuzyceWebApp.csproj", "LuzyceWebApp/"]
RUN dotnet restore "./LuzyceWebApp/LuzyceWebApp.csproj"

COPY . .
WORKDIR "/src/LuzyceWebApp"
RUN dotnet build "./LuzyceWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./LuzyceWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the Nginx image to serve the static content
FROM nginx:alpine as final
WORKDIR /var/www/html

# Copy the published content from the publish stage to the nginx html directory
COPY --from=publish /app/publish/wwwroot .

# Setup nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 35133 for external access
EXPOSE 35133
