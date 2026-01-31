using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Domain.Interfaces;

namespace AcompanhamentoPaciente.Application.Services;

public class AuthService : IAuthService
{
    private readonly IPsicologoRepository _psicologoRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IPsicologoRepository psicologoRepository, IJwtService jwtService)
    {
        _psicologoRepository = psicologoRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var psicologo = await _psicologoRepository.GetByEmailAsync(request.Email);

        if (psicologo == null || psicologo.Password != request.Password)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(psicologo.Id, psicologo.Email, psicologo.Nome);
        
        return new LoginResponse(token, psicologo.Nome);
    }
}
