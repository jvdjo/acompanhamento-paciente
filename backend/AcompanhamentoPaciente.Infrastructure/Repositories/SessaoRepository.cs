using AcompanhamentoPaciente.Domain.Entities;
using AcompanhamentoPaciente.Domain.Interfaces;
using AcompanhamentoPaciente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcompanhamentoPaciente.Infrastructure.Repositories;

public class SessaoRepository : Repository<Sessao>, ISessaoRepository
{
    public SessaoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Sessao>> GetByPacienteIdAsync(Guid pacienteId)
    {
        return await _dbSet
            .Where(s => s.PacienteId == pacienteId)
            .ToListAsync();
    }

    public async Task<Sessao?> GetByIdAndPacienteIdAsync(Guid id, Guid pacienteId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Id == id && s.PacienteId == pacienteId);
    }
}
