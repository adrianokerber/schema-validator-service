FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SchemaValidator.HttpService/SchemaValidator.HttpService.csproj", "SchemaValidator.HttpService/"]
RUN dotnet restore "SchemaValidator.HttpService/SchemaValidator.HttpService.csproj"
COPY . .
WORKDIR "/src/SchemaValidator.HttpService"
RUN dotnet build "SchemaValidator.HttpService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SchemaValidator.HttpService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchemaValidator.HttpService.dll"]
