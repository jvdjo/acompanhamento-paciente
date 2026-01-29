namespace AcompanhamentoPaciente.Api.Models;

public class Psicologo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
