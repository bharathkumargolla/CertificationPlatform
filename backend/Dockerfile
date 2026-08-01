FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Certification.slnx ./
COPY src/Certification.Api/Certification.Api.csproj src/Certification.Api/
COPY src/Certification.Application/Certification.Application.csproj src/Certification.Application/
COPY src/Certification.Domain/Certification.Domain.csproj src/Certification.Domain/
COPY src/Certification.Infrastructure/Certification.Infrastructure.csproj src/Certification.Infrastructure/
COPY src/Certification.Shared/Certification.Shared.csproj src/Certification.Shared/
RUN dotnet restore src/Certification.Api/Certification.Api.csproj

COPY src/ src/
RUN dotnet publish src/Certification.Api/Certification.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser
USER appuser

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Certification.Api.dll"]
