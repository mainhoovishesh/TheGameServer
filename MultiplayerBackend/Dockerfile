FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MultiplayerBackend/MultiplayerBackend.csproj", "MultiplayerBackend/"]
RUN dotnet restore "./MultiplayerBackend/./MultiplayerBackend.csproj"
COPY . .
WORKDIR "/src/MultiplayerBackend"
RUN dotnet build "./MultiplayerBackend.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MultiplayerBackend.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultiplayerBackend.dll"]