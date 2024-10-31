# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore ./PostService.csproj

# Copy the rest of the source code and build the app
COPY . ./
RUN dotnet publish -c Release -o /app/out ./PostService.csproj

# Stage 2: Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out /app

# Start the application
ENTRYPOINT ["dotnet", "PostService.dll"]