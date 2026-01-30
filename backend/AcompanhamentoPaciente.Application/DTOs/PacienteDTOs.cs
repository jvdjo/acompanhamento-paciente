namespace AcompanhamentoPaciente.Application.DTOs;

public record PacienteDto(int Id, string Nome, DateTime DataCadastro);

public record CreatePacienteRequest(string Nome);
