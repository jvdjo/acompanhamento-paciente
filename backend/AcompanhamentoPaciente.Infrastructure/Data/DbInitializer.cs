using AcompanhamentoPaciente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AcompanhamentoPaciente.Domain.Entities;

namespace AcompanhamentoPaciente.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        // context.Database.Migrate();
    }
}
