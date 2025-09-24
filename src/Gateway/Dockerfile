FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Gateway/*.csproj ./Gateway/
RUN dotnet restore ./Gateway/Gateway.csproj

COPY Gateway/. ./Gateway/
RUN dotnet publish ./Gateway/Gateway.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8081

ENTRYPOINT ["dotnet", "Gateway.dll"]