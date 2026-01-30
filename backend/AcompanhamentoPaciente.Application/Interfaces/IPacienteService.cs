using AcompanhamentoPaciente.Application.DTOs;

namespace AcompanhamentoPaciente.Application.Interfaces;

public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> GetAllByPsicologoAsync(int psicologoId);
    Task<PacienteDto?> GetByIdAsync(int id, int psicologoId);
    Task<PacienteDto> CreateAsync(CreatePacienteRequest request, int psicologoId);
    Task<bool> DeleteAsync(int id, int psicologoId);
}
