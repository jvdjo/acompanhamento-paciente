using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcompanhamentoPaciente.Api.Data;
using AcompanhamentoPaciente.Api.DTOs;
using AcompanhamentoPaciente.Api.Services;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var psicologo = await _context.Psicologos
            .FirstOrDefaultAsync(p => p.Email == request.Email);

        if (psicologo == null || !BCrypt.Net.BCrypt.Verify(request.Password, psicologo.PasswordHash))
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }

        var token = _jwtService.GenerateToken(psicologo.Id, psicologo.Email, psicologo.Nome);
        
        return Ok(new LoginResponse(token, psicologo.Nome));
    }
}
