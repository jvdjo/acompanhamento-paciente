namespace AcompanhamentoPaciente.Domain.Entities;

public class Sessao : EntityBase
{
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public string? Anotacoes { get; set; }
    public string? NotasTexto { get; set; }
    
    public Guid PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
}
