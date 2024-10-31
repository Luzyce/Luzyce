FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 35132

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["apps/src/Luzyce.Api/Luzyce.Api.csproj", "apps/src/Luzyce.Api/"]
COPY ["shared/src/Luzyce.Core/Luzyce.Core.csproj", "shared/src/Luzyce.Core/"]
COPY ["shared/src/Luzyce.Api.Core/Luzyce.Api.Core.csproj", "shared/src/Luzyce.Api.Core/"]
COPY ["shared/src/Luzyce.Api.Repositories/Luzyce.Api.Repositories.csproj", "shared/src/Luzyce.Api.Repositories/"]
COPY ["shared/src/Luzyce.Api.Domain/Luzyce.Api.Domain.csproj", "shared/src/Luzyce.Api.Domain/"]
COPY ["shared/src/Luzyce.Api.Db.AppDb/Luzyce.Api.Db.AppDb.csproj", "shared/src/Luzyce.Api.Db.AppDb/"]
COPY ["shared/src/Luzyce.Api.Db.Subiekt/Luzyce.Api.Db.Subiekt.csproj", "shared/src/Luzyce.Api.Db.Subiekt/"]
RUN dotnet restore "apps/src/Luzyce.Api/Luzyce.Api.csproj"
COPY . .
WORKDIR "/src/apps/src/Luzyce.Api"
RUN dotnet build "Luzyce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Luzyce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Luzyce.Api.dll"]
