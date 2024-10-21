FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 35132

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LuzyceApi/LuzyceApi.csproj", "LuzyceApi/"]
COPY ["LuzyceApi.Repositories/LuzyceApi.Repositories.csproj", "LuzyceApi.Repositories/"]
COPY ["LuzyceApi.Domain/LuzyceApi.Domain.csproj", "LuzyceApi.Domain/"]
COPY ["LuzyceApi.Db.AppDb/LuzyceApi.Db.AppDb.csproj", "LuzyceApi.Db.AppDb/"]
COPY ["LuzyceApi.Db.Subiekt/LuzyceApi.Db.Subiekt.csproj", "LuzyceApi.Db.Subiekt/"]
RUN dotnet restore "./LuzyceApi/LuzyceApi.csproj"
COPY . .
WORKDIR "/src/LuzyceApi"
RUN dotnet build "./LuzyceApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LuzyceApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LuzyceApi.dll"]