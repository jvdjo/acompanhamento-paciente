using AcompanhamentoPaciente.Domain.Entities;
using AcompanhamentoPaciente.Domain.Interfaces;
using AcompanhamentoPaciente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcompanhamentoPaciente.Infrastructure.Repositories;

public class PsicologoRepository : Repository<Psicologo>, IPsicologoRepository
{
    public PsicologoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Psicologo?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Email == email);
    }
}
