# ============================
# STAGE 1: RESTORE + BUILD
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Api/*.csproj ./Api/

RUN dotnet restore Api/Api.csproj

COPY . .

RUN dotnet build Api/Api.csproj -c Release -o /app/build

# ============================
# STAGE 2: PUBLISH
# ============================
FROM build AS publish
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ============================
# STAGE 3: RUNTIME
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .


ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_EnableDiagnostics=0

EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]
