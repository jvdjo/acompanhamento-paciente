namespace AcompanhamentoPaciente.Application.DTOs;

public record PacienteDto(Guid Id, string Nome, DateTime DataCadastro, string Profissao, string Escolaridade, DateTime DataNascimento, string Genero, int Idade);

public record CreatePacienteRequest(string Nome, string Profissao, string Escolaridade, DateTime DataNascimento, string Genero);
