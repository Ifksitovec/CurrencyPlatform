FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY UserService/UserService.Api/*.csproj UserService.Api/
COPY UserService/UserService.Application/*.csproj UserService.Application/
COPY UserService/UserService.Infrastructure/*.csproj UserService.Infrastructure/
COPY UserService/UserService.Domain/*.csproj UserService.Domain/

RUN dotnet nuget locals all --clear
RUN dotnet restore UserService.Api/UserService.Api.csproj

COPY UserService/UserService.Api/ UserService.Api/
COPY UserService/UserService.Application/ UserService.Application/
COPY UserService/UserService.Infrastructure/ UserService.Infrastructure/
COPY UserService/UserService.Domain/ UserService.Domain/

RUN dotnet publish UserService.Api/UserService.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "UserService.Api.dll"]