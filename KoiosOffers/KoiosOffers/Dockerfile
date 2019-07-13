FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["KoiosOffers/KoiosOffers.csproj", "KoiosOffers/"]
RUN dotnet restore "KoiosOffers/KoiosOffers.csproj"
COPY . .
WORKDIR "/src/KoiosOffers"
RUN dotnet build "KoiosOffers.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KoiosOffers.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KoiosOffers.dll"]