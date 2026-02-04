FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Render требует слушать порт из переменной PORT
ENV ASPNETCORE_URLS=http://+:$PORT

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MobileApp.dll"]