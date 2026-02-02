# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first for better caching
# Note: Paths are relative to the build context (repo root)
COPY ["backend/AcompanhamentoPaciente.sln", "backend/"]
COPY ["backend/AcompanhamentoPaciente.Api/AcompanhamentoPaciente.Api.csproj", "backend/AcompanhamentoPaciente.Api/"]
COPY ["backend/AcompanhamentoPaciente.Application/AcompanhamentoPaciente.Application.csproj", "backend/AcompanhamentoPaciente.Application/"]
COPY ["backend/AcompanhamentoPaciente.Domain/AcompanhamentoPaciente.Domain.csproj", "backend/AcompanhamentoPaciente.Domain/"]
COPY ["backend/AcompanhamentoPaciente.Infrastructure/AcompanhamentoPaciente.Infrastructure.csproj", "backend/AcompanhamentoPaciente.Infrastructure/"]

# Restore dependencies
WORKDIR /src/backend
RUN dotnet restore

# Copy the rest of the source code
COPY backend/ .

# Build and Publish
WORKDIR "/src/backend/AcompanhamentoPaciente.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render listens on 8080 by default, setting ASPNETCORE_HTTP_PORTS handles this)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AcompanhamentoPaciente.Api.dll"]
