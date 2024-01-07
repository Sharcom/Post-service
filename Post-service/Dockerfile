#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 6003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Post-service/Post-service.csproj", "Post-service/"]
RUN dotnet restore "Post-service/Post-service.csproj"
COPY . .
WORKDIR "/src/Post-service"
RUN dotnet build "Post-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Post-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Post-service.dll"]