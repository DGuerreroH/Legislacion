# ========== build ==========
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo el csproj primero (cachea restore)
COPY ["LegislacionAPP2025/LegislacionAPP.csproj", "LegislacionAPP/"]
RUN dotnet restore "LegislacionAPP2025/LegislacionAPP.csproj"

# Copia el resto del código y publica
COPY . .
WORKDIR /src/LegislacionAPP
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ========== runtime ==========
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LegislacionAPP.dll"]
