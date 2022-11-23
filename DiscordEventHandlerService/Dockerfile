FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DiscordEventHandlerService/DiscordEventHandlerService.csproj", "DiscordEventHandlerService/"]
RUN dotnet restore "DiscordEventHandlerService/DiscordEventHandlerService.csproj"
COPY . .
WORKDIR "/src/DiscordEventHandlerService"
RUN dotnet build "DiscordEventHandlerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordEventHandlerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordEventHandlerService.dll"]
