# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de proyecto y restaura dependencias
COPY *.sln ./
COPY Api/*.csproj ./Api/
RUN dotnet restore Api/Api.csproj

# Copia todo el código fuente y compila
COPY Api/. ./Api/
WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]