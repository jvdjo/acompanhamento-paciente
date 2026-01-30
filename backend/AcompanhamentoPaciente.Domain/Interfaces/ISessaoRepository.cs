using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Domain.Interfaces;

public interface ISessaoRepository : IRepository<Sessao>
{
    Task<IEnumerable<Sessao>> GetByPacienteIdAsync(int pacienteId);
    Task<Sessao?> GetByIdAndPacienteIdAsync(int id, int pacienteId);
}
