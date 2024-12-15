FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/Heyer.Storage.API/Heyer.Storage.API.csproj", "src/API/Heyer.Storage.API/"]
COPY ["src/API/Heyer.Storage.API.Client/Heyer.Storage.API.Client.csproj", "src/API/Heyer.Storage.API.Client/"]
COPY ["src/BuildingBlocks/Heyer.BuildingBlocks.Application/Heyer.BuildingBlocks.Application.csproj", "src/BuildingBlocks/Heyer.BuildingBlocks.Application/"]
COPY ["src/BuildingBlocks/Heyer.BuildingBlocks.Infrastructure/Heyer.BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks/Heyer.BuildingBlocks.Infrastructure/"]
COPY ["src/BuildingBlocks/Heyer.BuildingBlocks.Domain/Heyer.BuildingBlocks.Domain.csproj", "src/BuildingBlocks/Heyer.BuildingBlocks.Domain/"]
RUN dotnet restore "src/API/Heyer.Storage.API/Heyer.Storage.API.csproj"
COPY . .
WORKDIR "/src/src/API/Heyer.Storage.API"
RUN dotnet build "Heyer.Storage.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Heyer.Storage.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heyer.Storage.API.dll"]
