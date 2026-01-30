using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcompanhamentoPaciente.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<ISessaoService, SessaoService>();
        
        return services;
    }
}
