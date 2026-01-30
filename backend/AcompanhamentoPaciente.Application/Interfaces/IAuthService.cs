using AcompanhamentoPaciente.Application.DTOs;

namespace AcompanhamentoPaciente.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
