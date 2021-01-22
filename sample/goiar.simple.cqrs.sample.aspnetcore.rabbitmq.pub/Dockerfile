#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["sample/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.csproj", "sample/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub/"]
COPY ["src/goiar.simple.cqrs.persistance.rabbitmq/Goiar.Simple.Cqrs.persistance.rabbitmq.csproj", "./src/goiar.simple.cqrs.persistance.rabbitmq/"]
COPY ["src/goiar.simple.cqrs/goiar.simple.cqrs.csproj", "./src/goiar.simple.cqrs/"]
RUN dotnet restore "sample/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.csproj"
COPY . .
WORKDIR "/src/sample/goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub"
RUN dotnet build "goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.dll"]
