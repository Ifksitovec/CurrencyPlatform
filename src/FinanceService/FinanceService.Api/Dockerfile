FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY FinanceService/FinanceService.Api/*.csproj ./FinanceService.Api/
COPY FinanceService/FinanceService.Application/*.csproj ./FinanceService.Application/
COPY FinanceService/FinanceService.Infrastructure/*.csproj ./FinanceService.Infrastructure/
COPY FinanceService/FinanceService.Domain/*.csproj ./FinanceService.Domain/

RUN dotnet nuget locals all --clear
RUN dotnet restore FinanceService.Api/FinanceService.Api.csproj

COPY FinanceService/FinanceService.Api/ ./FinanceService.Api/
COPY FinanceService/FinanceService.Application/ ./FinanceService.Application/
COPY FinanceService/FinanceService.Infrastructure/ ./FinanceService.Infrastructure/
COPY FinanceService/FinanceService.Domain/ ./FinanceService.Domain/

RUN dotnet publish FinanceService.Api/FinanceService.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "FinanceService.Api.dll"]