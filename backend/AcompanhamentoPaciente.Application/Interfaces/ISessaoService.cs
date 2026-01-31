using AcompanhamentoPaciente.Application.DTOs;

namespace AcompanhamentoPaciente.Application.Interfaces;

public interface ISessaoService
{
    Task<IEnumerable<SessaoDto>> GetAllByPacienteAsync(Guid pacienteId, Guid psicologoId);
    Task<SessaoDto?> GetByIdAsync(Guid id, Guid pacienteId, Guid psicologoId);
    Task<SessaoDto?> CreateAsync(CreateSessaoRequest request, Guid pacienteId, Guid psicologoId);
    Task<bool> UpdateAsync(Guid id, UpdateSessaoRequest request, Guid pacienteId, Guid psicologoId);
    Task<bool> DeleteAsync(Guid id, Guid pacienteId, Guid psicologoId);
}
