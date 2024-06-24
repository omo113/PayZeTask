#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PayZe.Payment.Api/PayZe.Payment.Api.csproj", "PayZe.Payment.Api/"]
COPY ["PayZe.Orders.Application/PayZe.Orders.Application.csproj", "PayZe.Orders.Application/"]
COPY ["PayZe.Contracts/PayZe.Contracts.csproj", "PayZe.Contracts/"]
COPY ["PayZe.Shared/PayZe.Shared.csproj", "PayZe.Shared/"]
COPY ["PayZe.Orders.Domain/PayZe.Orders.Domain.csproj", "PayZe.Orders.Domain/"]
COPY ["PayZe.Orders.Infrastructure/PayZe.Orders.Infrastructure.csproj", "PayZe.Orders.Infrastructure/"]
COPY ["PayZe.Payment.Application/PayZe.Payment.Application.csproj", "PayZe.Payment.Application/"]
COPY ["PayZe.Payment.Domain/PayZe.Payment.Domain.csproj", "PayZe.Payment.Domain/"]
COPY ["PayZe.Payment.Infrastructure/PayZe.Payment.Infrastructure.csproj", "PayZe.Payment.Infrastructure/"]
COPY ["PayZe.Payment.RabbitMq/PayZe.Payment.RabbitMq.csproj", "PayZe.Payment.RabbitMq/"]
RUN dotnet restore "./PayZe.Payment.Api/PayZe.Payment.Api.csproj"
COPY . .
WORKDIR "/src/PayZe.Payment.Api"
RUN dotnet build "./PayZe.Payment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PayZe.Payment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PayZe.Payment.Api.dll"]