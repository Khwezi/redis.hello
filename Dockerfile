FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Redis.Hello/Redis.Hello.csproj", "Redis.Hello/"]
RUN dotnet restore "Redis.Hello/Redis.Hello.csproj"
COPY . .
WORKDIR "/src/Redis.Hello"
RUN dotnet build "Redis.Hello.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Redis.Hello.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Redis.Hello.dll"]