FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo el csproj primero (cachea restore)
COPY ["LegislacionAPP/LegislacionAPP.csproj", "LegislacionAPP/"]
RUN dotnet restore "LegislacionAPP/LegislacionAPP.csproj"

# Copia el resto del código y publica
COPY . .

# El .csproj está en la RAÍZ del repo
RUN dotnet restore ./LegislacionAPP.csproj
RUN dotnet publish ./LegislacionAPP.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LegislacionAPP.dll"]
