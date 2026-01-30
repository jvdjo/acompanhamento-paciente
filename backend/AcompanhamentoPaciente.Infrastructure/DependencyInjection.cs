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
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IPsicologoRepository, PsicologoRepository>();
        services.AddScoped<ISessaoRepository, SessaoRepository>();
        
        return services;
    }
}
