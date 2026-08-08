
EXPOSE 8080
RUN adduser --disabled-password --gecos "" appuser

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BuyMoreApi.API/BuyMoreApi.API.csproj", "BuyMoreApi.API/"]
COPY ["BuyMoreApi.Application/BuyMoreApi.Application.csproj", "BuyMoreApi.Application/"]
COPY ["BuyMoreApi.Domain/BuyMoreApi.Domain.csproj", "BuyMoreApi.Domain/"]
COPY ["BuyMoreApi.Infrastructure/BuyMoreApi.Infrastructure.csproj", "BuyMoreApi.Infrastructure/"]
RUN dotnet restore "BuyMoreApi.API/BuyMoreApi.API.csproj"

COPY . .
WORKDIR /src/BuyMoreApi.API
RUN dotnet build "BuyMoreApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BuyMoreApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/Storage && chown -R appuser:appuser /app
USER appuser
ENTRYPOINT ["dotnet", "BuyMoreApi.API.dll"]
