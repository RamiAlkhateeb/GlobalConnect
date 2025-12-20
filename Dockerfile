# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and project files
COPY *.sln .
COPY GC.Domain/*.csproj GC.Domain/
COPY GC.Application/*.csproj GC.Application/
COPY GC.Infrastructure/*.csproj GC.Infrastructure/
COPY GC.API/*.csproj GC.API/

# Copy everything else
COPY . .

# Restore dependencies
RUN dotnet restore GlobalConnect.sln
# WORKDIR /app/src/GC.API
RUN dotnet publish -c Release -o /out GC.API/API.csproj

# STAGE 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /out .

# Copy your Static JSON data files (Important for your Reference Data logic)
# COPY --from=build /app/src/GlobalConnect.API/Data/Static ./Data/Static

# Set environment variables for Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "API.dll"]