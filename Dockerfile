# This is the Dockerfile from 
# [Tutorial: Containerize a .NET app](https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux)

#
# Build this image with
#
#   docker build -t dotnet-dev:0.0.1 -f Dockerfile .
#

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -r linux-x64 --self-contained true -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]