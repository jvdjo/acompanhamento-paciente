namespace AcompanhamentoPaciente.Application.DTOs;

public record PacienteDto(Guid Id, string Nome, DateTime DataCadastro);

public record CreatePacienteRequest(string Nome);
