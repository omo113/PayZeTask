#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PayZE.Company.API/PayZe.Identity.Api.csproj", "PayZE.Company.API/"]
COPY ["PayZe.Identity.Application/PayZe.Identity.Application.csproj", "PayZe.Identity.Application/"]
COPY ["PayZe.Identity.Infrastructure/PayZe.Identity.Infrastructure.csproj", "PayZe.Identity.Infrastructure/"]
COPY ["PayZe.Identity.Domain/PayZe.Identity.Domain.csproj", "PayZe.Identity.Domain/"]
COPY ["PayZe.Contracts/PayZe.Contracts.csproj", "PayZe.Contracts/"]
COPY ["PayZe.Shared/PayZe.Shared.csproj", "PayZe.Shared/"]
COPY ["PayZe.Identity.RabbitMq/PayZe.Identity.RabbitMq.csproj", "PayZe.Identity.RabbitMq/"]
RUN dotnet restore "./PayZE.Company.API/PayZe.Identity.Api.csproj"
COPY . .
WORKDIR "/src/PayZE.Company.API"
RUN dotnet build "./PayZe.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PayZe.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayZe.Identity.Api.dll"]