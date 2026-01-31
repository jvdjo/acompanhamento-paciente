using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Domain.Interfaces;

public interface ISessaoRepository : IRepository<Sessao>
{
    Task<IEnumerable<Sessao>> GetByPacienteIdAsync(Guid pacienteId);
    Task<Sessao?> GetByIdAndPacienteIdAsync(Guid id, Guid pacienteId);
}
