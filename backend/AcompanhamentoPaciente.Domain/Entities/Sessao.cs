namespace AcompanhamentoPaciente.Domain.Entities;

public class Sessao
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public string? Anotacoes { get; set; }
    
    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
}
