# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY src/GlobalConnect.Domain/*.csproj src/GlobalConnect.Domain/
COPY src/GlobalConnect.Application/*.csproj src/GlobalConnect.Application/
COPY src/GlobalConnect.Infrastructure/*.csproj src/GlobalConnect.Infrastructure/
COPY src/GlobalConnect.API/*.csproj src/GlobalConnect.API/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/src/GlobalConnect.API
RUN dotnet publish -c Release -o /out

# STAGE 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /out .

# Copy your Static JSON data files (Important for your Reference Data logic)
COPY --from=build /app/src/GlobalConnect.API/Data/Static ./Data/Static

# Set environment variables for Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "GlobalConnect.API.dll"]