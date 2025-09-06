# syntax=docker/dockerfile:1

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# si el csproj está en la raíz del repo/proyecto:
COPY ["LegislacionAPP/LegislacionAPP.csproj", "."]
RUN dotnet restore "LegislacionAPP/LegislacionAPP.csproj"

# copia el resto del código
COPY . .
RUN dotnet build "LegislacionAPP/LegislacionAPP.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "LegislacionAPP/LegislacionAPP.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegislacionAPP.dll"]
