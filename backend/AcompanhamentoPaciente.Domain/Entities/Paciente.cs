namespace AcompanhamentoPaciente.Domain.Entities;

public class Paciente : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public string Profissao { get; set; } = string.Empty;
    public string Escolaridade { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Genero { get; set; } = string.Empty;
    
    public Guid PsicologoId { get; set; }
    public Psicologo? Psicologo { get; set; }
    
    public ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
}
