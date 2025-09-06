# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LegislacionAPP/LegislacionAPP.csproj", "LegislacionAPP/"]
RUN dotnet restore "LegislacionAPP/LegislacionAPP.csproj"
COPY . .
WORKDIR /src/LegislacionAPP
RUN dotnet build "LegislacionAPP.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/LegislacionAPP
RUN dotnet publish "LegislacionAPP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegislacionAPP.dll"]
