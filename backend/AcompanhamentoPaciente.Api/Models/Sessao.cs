namespace AcompanhamentoPaciente.Api.Models;

public class Sessao
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public string? Anotacoes { get; set; } // JSON/Base64 para dados do canvas
    
    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
}
