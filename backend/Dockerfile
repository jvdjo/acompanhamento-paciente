# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first for better caching
COPY ["AcompanhamentoPaciente.sln", "./"]
COPY ["AcompanhamentoPaciente.Api/AcompanhamentoPaciente.Api.csproj", "AcompanhamentoPaciente.Api/"]
COPY ["AcompanhamentoPaciente.Application/AcompanhamentoPaciente.Application.csproj", "AcompanhamentoPaciente.Application/"]
COPY ["AcompanhamentoPaciente.Domain/AcompanhamentoPaciente.Domain.csproj", "AcompanhamentoPaciente.Domain/"]
COPY ["AcompanhamentoPaciente.Infrastructure/AcompanhamentoPaciente.Infrastructure.csproj", "AcompanhamentoPaciente.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and Publish
WORKDIR "/src/AcompanhamentoPaciente.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render listens on 8080 by default, setting ASPNETCORE_HTTP_PORTS handles this)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AcompanhamentoPaciente.Api.dll"]
