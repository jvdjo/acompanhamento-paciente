namespace AcompanhamentoPaciente.Application.DTOs;

public record SessaoDto(Guid Id, DateTime Data, string? Anotacoes, string? NotasTexto);

public record CreateSessaoRequest(DateTime? Data);

public record UpdateSessaoRequest(string? Anotacoes, string? NotasTexto);
