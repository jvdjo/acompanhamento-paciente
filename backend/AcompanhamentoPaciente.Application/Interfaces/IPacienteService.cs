using AcompanhamentoPaciente.Application.DTOs;

namespace AcompanhamentoPaciente.Application.Interfaces;

public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> GetAllByPsicologoAsync(Guid psicologoId);
    Task<PacienteDto?> GetByIdAsync(Guid id, Guid psicologoId);
    Task<PacienteDto> CreateAsync(CreatePacienteRequest request, Guid psicologoId);
    Task<bool> DeleteAsync(Guid id, Guid psicologoId);
}
