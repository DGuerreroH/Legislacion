# syntax=docker/dockerfile:1

# ===== RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# ===== BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos SOLO el csproj para aprovechar la caché de Docker
COPY ["LegislacionAPP.csproj", "."]
RUN dotnet restore "LegislacionAPP.csproj"

# Copiamos el resto del código
COPY . .
RUN dotnet build "LegislacionAPP.csproj" -c Release -o /app/build

# ===== PUBLISH =====
FROM build AS publish
RUN dotnet publish "LegislacionAPP.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== FINAL =====
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegislacionAPP.dll"]
