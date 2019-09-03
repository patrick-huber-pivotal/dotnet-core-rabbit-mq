FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["RabbitMqPcf.Web/RabbitMqPcf.Web/RabbitMqPcf.Web.csproj", "RabbitMqPcf.Web/RabbitMqPcf.Web/"]
RUN dotnet restore "RabbitMqPcf.Web/RabbitMqPcf.Web/RabbitMqPcf.Web.csproj"
COPY . .
WORKDIR "/src/RabbitMqPcf.Web/RabbitMqPcf.Web"
RUN dotnet build "RabbitMqPcf.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RabbitMqPcf.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RabbitMqPcf.Web.dll"]
