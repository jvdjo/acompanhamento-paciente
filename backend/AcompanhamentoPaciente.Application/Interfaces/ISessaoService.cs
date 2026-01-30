using AcompanhamentoPaciente.Application.DTOs;

namespace AcompanhamentoPaciente.Application.Interfaces;

public interface ISessaoService
{
    Task<IEnumerable<SessaoDto>> GetAllByPacienteAsync(int pacienteId, int psicologoId);
    Task<SessaoDto?> GetByIdAsync(int id, int pacienteId, int psicologoId);
    Task<SessaoDto?> CreateAsync(CreateSessaoRequest request, int pacienteId, int psicologoId);
    Task<bool> UpdateAsync(int id, UpdateSessaoRequest request, int pacienteId, int psicologoId);
    Task<bool> DeleteAsync(int id, int pacienteId, int psicologoId);
}
