FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet publish \
    ErpAcademico.WebApi/ErpAcademico.WebApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore
    
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "ErpAcademico.WebApi.dll"]