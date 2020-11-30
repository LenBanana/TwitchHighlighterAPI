FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TwitchHighlighterAPI/TwitchHighlighterAPI.csproj", "TwitchHighlighterAPI/"]
RUN dotnet restore "TwitchHighlighterAPI/TwitchHighlighterAPI.csproj"
COPY . .
WORKDIR "/src/TwitchHighlighterAPI"
RUN dotnet build "TwitchHighlighterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitchHighlighterAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitchHighlighterAPI.dll"]