namespace AcompanhamentoPaciente.Application.DTOs;



public record LoginResponse(string Token, string Nome, string? Picture = null);

public record GoogleUserInfo(string GoogleId, string Email, string Nome, string? Picture);
