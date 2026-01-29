using Microsoft.AspNetCore.Mvc;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetupController : ControllerBase
{
    private readonly Data.AppDbContext _context;

    public SetupController(Data.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPassword()
    {
        var admin = await _context.Psicologos.FindAsync(1);
        if (admin == null)
        {
            admin = new Models.Psicologo
            {
                Id = 1,
                Nome = "Dr. Admin",
                Email = "admin@clinica.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            };
            _context.Psicologos.Add(admin);
        }
        else
        {
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
        }
        
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Senha resetada para admin123", hash = admin.PasswordHash });
    }
}
