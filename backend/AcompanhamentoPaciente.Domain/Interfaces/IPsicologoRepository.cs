using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Domain.Interfaces;

public interface IPsicologoRepository : IRepository<Psicologo>
{
    Task<Psicologo?> GetByEmailAsync(string email);
}
