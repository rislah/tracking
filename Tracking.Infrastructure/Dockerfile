FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tracking.Infrastructure/Tracking.Infrastructure.csproj", "Tracking.Infrastructure/"]
RUN dotnet restore "Tracking.Infrastructure/Tracking.Infrastructure.csproj"
COPY . .
WORKDIR "/src/Tracking.Infrastructure"
RUN dotnet build "Tracking.Infrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracking.Infrastructure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracking.Infrastructure.dll"]
