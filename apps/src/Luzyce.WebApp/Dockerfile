FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 35133

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["apps/src/Luzyce.WebApp/Luzyce.WebApp.csproj", "apps/src/Luzyce.WebApp/"]
COPY ["shared/src/Luzyce.Core/Luzyce.Core.csproj", "shared/src/Luzyce.Core/"]
RUN dotnet restore "apps/src/Luzyce.WebApp/Luzyce.WebApp.csproj"
COPY . .
WORKDIR "/src/apps/src/Luzyce.WebApp"
RUN dotnet build "Luzyce.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Luzyce.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine as final
WORKDIR /var/www/html

COPY --from=publish /app/publish/wwwroot .

COPY apps/src/Luzyce.WebApp/nginx.conf /etc/nginx/nginx.conf


