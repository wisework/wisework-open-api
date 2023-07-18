FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG ENVIRONMENT=Development
ARG NUGET_USERNAME
ARG NUGET_PASSWORD
ENV ASPNETCORE_URLS=http://+:5000 
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

WORKDIR /src
COPY . /src/
RUN dotnet new nugetconfig && \
  dotnet nuget add source --username $NUGET_USERNAME \
  --password $NUGET_PASSWORD \
  --store-password-in-clear-text \
  --name github "https://nuget.pkg.github.com/wisework/index.json"

RUN dotnet restore
RUN dotnet publish -c Release -o app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
EXPOSE 5000
COPY --from=build /src/app ./
ENTRYPOINT ["dotnet", "OpenAPI.dll"] 