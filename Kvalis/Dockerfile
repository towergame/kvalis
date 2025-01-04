FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Kvalis/Kvalis.csproj", "Kvalis/"]
RUN dotnet restore "Kvalis/Kvalis.csproj"
COPY . .
WORKDIR "/src/Kvalis"
RUN dotnet build "Kvalis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kvalis.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kvalis.dll"]
