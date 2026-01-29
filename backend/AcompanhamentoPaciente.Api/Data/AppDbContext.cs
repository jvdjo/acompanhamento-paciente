using Microsoft.EntityFrameworkCore;
using AcompanhamentoPaciente.Api.Models;

namespace AcompanhamentoPaciente.Api.Data;

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

        // Seed default psychologist
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        modelBuilder.Entity<Psicologo>().HasData(new Psicologo
        {
            Id = 1,
            Nome = "Dr. Admin",
            Email = "admin@clinica.com",
            PasswordHash = hashedPassword
        });
    }
}
