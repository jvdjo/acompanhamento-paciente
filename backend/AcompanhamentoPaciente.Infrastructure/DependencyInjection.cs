using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Domain.Interfaces;
using AcompanhamentoPaciente.Infrastructure.Data;
using AcompanhamentoPaciente.Infrastructure.Repositories;
using AcompanhamentoPaciente.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcompanhamentoPaciente.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Parse Render/Heroku style URL: postgres://user:password@host:port/database
        if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
        {
            var databaseUri = new Uri(connectionString);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };
            connectionString = builder.ToString();
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IPsicologoRepository, PsicologoRepository>();
        services.AddScoped<ISessaoRepository, SessaoRepository>();
        
        return services;
    }
}
