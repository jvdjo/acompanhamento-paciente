namespace AcompanhamentoPaciente.Domain.Entities;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    
    public int PsicologoId { get; set; }
    public Psicologo? Psicologo { get; set; }
    
    public ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
}
