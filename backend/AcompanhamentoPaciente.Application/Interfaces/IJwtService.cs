namespace AcompanhamentoPaciente.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid psicologoId, string email, string nome);
}
