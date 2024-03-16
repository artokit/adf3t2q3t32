FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "EducationService/EducationService.csproj"
WORKDIR "/src/TgBotApi"
RUN dotnet build "EducationService.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/EducationService"
RUN dotnet publish "EducationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EducationService.dll"]