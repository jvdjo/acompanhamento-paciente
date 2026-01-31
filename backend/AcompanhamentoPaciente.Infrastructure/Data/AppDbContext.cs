using AcompanhamentoPaciente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcompanhamentoPaciente.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Psicologo> Psicologos => Set<Psicologo>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Sessao> Sessoes => Set<Sessao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Psicologo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.Psicologo)
                  .WithMany(p => p.Pacientes)
                  .HasForeignKey(e => e.PsicologoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Sessao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Paciente)
                  .WithMany(p => p.Sessoes)
                  .HasForeignKey(e => e.PacienteId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed default psychologist with pre-hashed password for "admin123"
        modelBuilder.Entity<Psicologo>().HasData(new Psicologo
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Nome = "Dr. Admin",
            Email = "admin@clinica.com",
            PasswordHash = "$2a$11$K5FxKqW0qKPe5jMB9sqxmu.XD6JT3.yk3EfJqrGUcZqnOBG9WFxVe"
        });
    }
}
