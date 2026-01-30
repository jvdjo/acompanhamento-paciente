using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Domain.Interfaces;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task<IEnumerable<Paciente>> GetByPsicologoIdAsync(int psicologoId);
    Task<Paciente?> GetByIdAndPsicologoIdAsync(int id, int psicologoId);
}
