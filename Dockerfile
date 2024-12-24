FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /app

COPY *.sln .
COPY src/QuizWebApp/*.csproj ./src/QuizWebApp/
RUN dotnet restore

COPY . .

ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app/src/QuizWebApp

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:80"]

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