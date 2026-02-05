using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Domain.Entities;
using AcompanhamentoPaciente.Domain.Interfaces;

namespace AcompanhamentoPaciente.Application.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository;

    public PacienteService(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<IEnumerable<PacienteDto>> GetAllByPsicologoAsync(Guid psicologoId)
    {
        var pacientes = await _pacienteRepository.GetByPsicologoIdAsync(psicologoId);
        return pacientes
            .OrderBy(p => p.Nome)
            .Select(p => new PacienteDto(
                p.Id, 
                p.Nome, 
                p.DataCadastro,
                p.Profissao,
                p.Escolaridade,
                p.DataNascimento,
                p.Genero,
                CalculateAge(p.DataNascimento)
            ));
    }

    public async Task<PacienteDto?> GetByIdAsync(Guid id, Guid psicologoId)
    {
        var paciente = await _pacienteRepository.GetByIdAndPsicologoIdAsync(id, psicologoId);
        if (paciente == null) return null;
        
        return new PacienteDto(
            paciente.Id, 
            paciente.Nome, 
            paciente.DataCadastro,
            paciente.Profissao,
            paciente.Escolaridade,
            paciente.DataNascimento,
            paciente.Genero,
            CalculateAge(paciente.DataNascimento)
        );
    }

    public async Task<PacienteDto> CreateAsync(CreatePacienteRequest request, Guid psicologoId)
    {
        var paciente = new Paciente
        {
            Nome = request.Nome,
            Profissao = request.Profissao,
            Escolaridade = request.Escolaridade,
            DataNascimento = request.DataNascimento,
            Genero = request.Genero,
            PsicologoId = psicologoId,
            DataCadastro = DateTime.UtcNow
        };

        await _pacienteRepository.AddAsync(paciente);
        
        return new PacienteDto(
            paciente.Id, 
            paciente.Nome, 
            paciente.DataCadastro,
            paciente.Profissao,
            paciente.Escolaridade,
            paciente.DataNascimento,
            paciente.Genero,
            CalculateAge(paciente.DataNascimento)
        );
    }

    private int CalculateAge(DateTime dataNascimento)
    {
        if (dataNascimento == default) return 0;
        
        var today = DateTime.Today;
        var age = today.Year - dataNascimento.Year;
        
        if (dataNascimento.Date > today.AddYears(-age)) age--;
        
        return age;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid psicologoId)
    {
        var paciente = await _pacienteRepository.GetByIdAndPsicologoIdAsync(id, psicologoId);
        if (paciente == null) return false;

        await _pacienteRepository.DeleteAsync(paciente);
        return true;
    }
}
