using AcompanhamentoPaciente.Domain.Entities;
using AcompanhamentoPaciente.Domain.Interfaces;
using AcompanhamentoPaciente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcompanhamentoPaciente.Infrastructure.Repositories;

public class PacienteRepository : Repository<Paciente>, IPacienteRepository
{
    public PacienteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Paciente>> GetByPsicologoIdAsync(Guid psicologoId)
    {
        return await _dbSet
            .Where(p => p.PsicologoId == psicologoId)
            .ToListAsync();
    }

    public async Task<Paciente?> GetByIdAndPsicologoIdAsync(Guid id, Guid psicologoId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Id == id && p.PsicologoId == psicologoId);
    }
}
