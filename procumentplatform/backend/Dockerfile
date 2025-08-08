# Backend Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["VWProcurement.API/VWProcurement.API.csproj", "VWProcurement.API/"]
COPY ["VWProcurement.Core/VWProcurement.Core.csproj", "VWProcurement.Core/"]
COPY ["VWProcurement.Data/VWProcurement.Data.csproj", "VWProcurement.Data/"]

RUN dotnet restore "VWProcurement.API/VWProcurement.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/VWProcurement.API"
RUN dotnet build "VWProcurement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VWProcurement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "VWProcurement.API.dll"]
