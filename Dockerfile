FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ErpAcademico.Domain/ ErpAcademico.Domain/
COPY ErpAcademico.Application/ ErpAcademico.Application/
COPY ErpAcademico.Infrastructure/ ErpAcademico.Infrastructure/
COPY ErpAcademico.WebApi/ ErpAcademico.WebApi/
COPY ErpAcademico.sln .

RUN dotnet publish ErpAcademico.WebApi/ErpAcademico.WebApi.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "ErpAcademico.WebApi.dll"]