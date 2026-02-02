using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }
        
        return Ok(response);
    }

    /// <summary>
    /// Inicia o fluxo de autenticação com Google OAuth2
    /// </summary>
    [HttpGet("google")]
    public IActionResult GoogleLogin([FromQuery] string? returnUrl = null)
    {
        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
        var redirectUrl = returnUrl ?? $"{frontendUrl}/auth/callback";
        
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback)),
            Items = { { "returnUrl", redirectUrl } }
        };
        
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Callback do Google OAuth2 - processa a resposta e redireciona para o frontend com o token
    /// </summary>
    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleCallback()
    {
        // Tenta autenticar usando o scheme de cookies (onde o Google salvou as claims)
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        if (!authenticateResult.Succeeded)
        {
            // Fallback para o scheme do Google
            authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        }
        
        if (!authenticateResult.Succeeded)
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            return Redirect($"{frontendUrl}/login?error=google_auth_failed");
        }

        var claims = authenticateResult.Principal?.Claims;
        
        var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value 
                   ?? claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

        if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email))
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            return Redirect($"{frontendUrl}/login?error=invalid_google_response");
        }

        var googleUser = new GoogleUserInfo(googleId, email, name ?? email, picture);
        var loginResponse = await _authService.LoginWithGoogleAsync(googleUser);

        // Recupera a URL de retorno ou usa o padrão
        var returnUrl = authenticateResult.Properties?.Items["returnUrl"] 
                     ?? $"{_configuration["FrontendUrl"] ?? "http://localhost:5173"}/auth/callback";
        
        // Limpa o cookie de autenticação temporário
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        // Redireciona para o frontend com o token
        var redirectUrl = $"{returnUrl}?token={loginResponse.Token}&nome={Uri.EscapeDataString(loginResponse.Nome)}";
        
        if (!string.IsNullOrEmpty(loginResponse.Picture))
        {
            redirectUrl += $"&picture={Uri.EscapeDataString(loginResponse.Picture)}";
        }
        
        return Redirect(redirectUrl);
    }
}
