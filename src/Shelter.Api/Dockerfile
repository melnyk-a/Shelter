FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Shelter.Api/Shelter.Api.csproj", "src/Shelter.Api/"]
COPY ["src/Shelter.Application/Shelter.Application.csproj", "src/Shelter.Application/"]
COPY ["src/Shelter.Domain/Shelter.Domain.csproj", "src/Shelter.Domain/"]
COPY ["src/Shelter.Infrastructure/Shelter.Infrastructure.csproj", "src/Shelter.Infrastructure/"]
COPY ["src/Shelter.Persistence/Shelter.Persistence.csproj", "src/Shelter.Persistence/"]
COPY ["src/Shelter.Auth.Keycloak/Shelter.Auth.Keycloak.csproj", "src/Shelter.Auth.Keycloak/"]
RUN dotnet restore "./src/Shelter.Api/./Shelter.Api.csproj"
COPY . .
WORKDIR "/src/src/Shelter.Api"
RUN dotnet build "./Shelter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Shelter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shelter.Api.dll"]