FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY IoC/IoC.csproj ./IoC/
COPY Presentation/Presentation.csproj ./Presentation/
RUN dotnet restore ./Presentation/Presentation.csproj

COPY . .
RUN dotnet publish ./Presentation/Presentation.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Presentation.dll"]
