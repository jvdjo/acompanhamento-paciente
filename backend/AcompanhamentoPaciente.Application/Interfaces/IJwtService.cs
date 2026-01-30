namespace AcompanhamentoPaciente.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(int psicologoId, string email, string nome);
}
