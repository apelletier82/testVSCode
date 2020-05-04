FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5010
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["TAPI2/TAPI2.csproj", "TAPI2/"]
RUN dotnet restore "TAPI2/TAPI2.csproj"
COPY . .
WORKDIR "/src/TAPI2"
RUN dotnet build "TAPI2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TAPI2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TAPI2.dll"]
