using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Domain.Entities;
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
        
        return new LoginResponse(token, psicologo.Nome, psicologo.Picture);
    }

    public async Task<LoginResponse> LoginWithGoogleAsync(GoogleUserInfo googleUser)
    {
        // Tenta encontrar pelo GoogleId primeiro
        var psicologo = await _psicologoRepository.GetByGoogleIdAsync(googleUser.GoogleId);

        if (psicologo == null)
        {
            // Tenta encontrar pelo email (pode ser uma conta existente vinculando ao Google)
            psicologo = await _psicologoRepository.GetByEmailAsync(googleUser.Email);

            if (psicologo != null)
            {
                // Vincula a conta existente ao Google
                psicologo.GoogleId = googleUser.GoogleId;
                psicologo.Picture = googleUser.Picture;
                await _psicologoRepository.UpdateAsync(psicologo);
            }
            else
            {
                // Cria um novo psicólogo
                psicologo = new Psicologo
                {
                    Id = Guid.NewGuid(),
                    Email = googleUser.Email,
                    Nome = googleUser.Nome,
                    GoogleId = googleUser.GoogleId,
                    Picture = googleUser.Picture,
                    Password = null // Sem senha para login via Google
                };
                await _psicologoRepository.AddAsync(psicologo);
            }
        }
        else
        {
            // Atualiza a foto do perfil se mudou
            if (psicologo.Picture != googleUser.Picture)
            {
                psicologo.Picture = googleUser.Picture;
                await _psicologoRepository.UpdateAsync(psicologo);
            }
        }

        var token = _jwtService.GenerateToken(psicologo.Id, psicologo.Email, psicologo.Nome);
        
        return new LoginResponse(token, psicologo.Nome, psicologo.Picture);
    }
}
