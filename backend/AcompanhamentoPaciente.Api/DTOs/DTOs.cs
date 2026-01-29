namespace AcompanhamentoPaciente.Api.DTOs;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string Nome);

public record PacienteDto(int Id, string Nome, DateTime DataCadastro);

public record CreatePacienteRequest(string Nome);

public record SessaoDto(int Id, DateTime Data, string? Anotacoes);

public record CreateSessaoRequest(DateTime? Data);

public record UpdateSessaoRequest(string? Anotacoes);
