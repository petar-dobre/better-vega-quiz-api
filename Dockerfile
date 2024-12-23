FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.sln .
COPY src/QuizWebApp/*.csproj ./src/QuizWebApp/
RUN dotnet restore

COPY . .
WORKDIR /app/src/QuizWebApp
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /QuizWebApp
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "QuizWebApp.dll"]