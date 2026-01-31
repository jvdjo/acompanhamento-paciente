using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Domain.Interfaces;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task<IEnumerable<Paciente>> GetByPsicologoIdAsync(Guid psicologoId);
    Task<Paciente?> GetByIdAndPsicologoIdAsync(Guid id, Guid psicologoId);
}
