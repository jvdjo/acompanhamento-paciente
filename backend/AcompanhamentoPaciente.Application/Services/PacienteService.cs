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
            .Select(p => new PacienteDto(p.Id, p.Nome, p.DataCadastro));
    }

    public async Task<PacienteDto?> GetByIdAsync(Guid id, Guid psicologoId)
    {
        var paciente = await _pacienteRepository.GetByIdAndPsicologoIdAsync(id, psicologoId);
        if (paciente == null) return null;
        
        return new PacienteDto(paciente.Id, paciente.Nome, paciente.DataCadastro);
    }

    public async Task<PacienteDto> CreateAsync(CreatePacienteRequest request, Guid psicologoId)
    {
        var paciente = new Paciente
        {
            Nome = request.Nome,
            PsicologoId = psicologoId,
            DataCadastro = DateTime.UtcNow
        };

        await _pacienteRepository.AddAsync(paciente);
        
        return new PacienteDto(paciente.Id, paciente.Nome, paciente.DataCadastro);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid psicologoId)
    {
        var paciente = await _pacienteRepository.GetByIdAndPsicologoIdAsync(id, psicologoId);
        if (paciente == null) return false;

        await _pacienteRepository.DeleteAsync(paciente);
        return true;
    }
}
