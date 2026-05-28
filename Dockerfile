FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["StressNull.Api/StressNull.Api.csproj", "StressNull.Api/"]
RUN dotnet restore "StressNull.Api/StressNull.Api.csproj"

# Copy the remaining source code
COPY . .

# Build and publish
RUN dotnet publish "StressNull.Api/StressNull.Api.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 8080 (the default for .NET 8+)
EXPOSE 8080

ENTRYPOINT ["dotnet", "StressNull.Api.dll"]