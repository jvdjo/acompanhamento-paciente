namespace AcompanhamentoPaciente.Domain.Entities;

public class Psicologo : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? GoogleId { get; set; }
    public string? Picture { get; set; }
    
    public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
