# ----------------------------------------------------
# Stage 1: Build & Publish (Uses full .NET SDK)
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy project definition and restore dependencies separately (caches layers)
COPY dotnet-example.csproj ./
RUN dotnet restore dotnet-example.csproj

# Copy the rest of the source code and compile
COPY . ./
RUN dotnet publish dotnet-example.csproj -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------------------------------
# Stage 2: Runtime (Lightweight production image)
# ----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

# Run as non-root user (built into .NET alpine images)
USER app

COPY --from=build /app/publish .

# ASP.NET Core in container binds to port 8080 by default
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "dotnet-example.dll"]
