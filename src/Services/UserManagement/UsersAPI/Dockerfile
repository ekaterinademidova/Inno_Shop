#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/UserManagement/UsersAPI/UsersAPI.csproj", "Services/UserManagement/UsersAPI/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Services/UserManagement/UsersApplication/UsersApplication.csproj", "Services/UserManagement/UsersApplication/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["Services/UserManagement/UsersDomain/UsersDomain.csproj", "Services/UserManagement/UsersDomain/"]
COPY ["Services/UserManagement/UsersInfrastucture/UsersInfrastucture.csproj", "Services/UserManagement/UsersInfrastucture/"]
RUN dotnet restore "./Services/UserManagement/UsersAPI/UsersAPI.csproj"
COPY . .
WORKDIR "/src/Services/UserManagement/UsersAPI"
RUN dotnet build "./UsersAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UsersAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersAPI.dll"]