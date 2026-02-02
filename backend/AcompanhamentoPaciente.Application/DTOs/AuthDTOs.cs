namespace AcompanhamentoPaciente.Application.DTOs;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string Nome, string? Picture = null);

public record GoogleUserInfo(string GoogleId, string Email, string Nome, string? Picture);
