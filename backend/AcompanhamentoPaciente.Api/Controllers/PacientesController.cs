using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcompanhamentoPaciente.Api.Data;
using AcompanhamentoPaciente.Api.DTOs;
using AcompanhamentoPaciente.Api.Models;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PacientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PacientesController(AppDbContext context)
    {
        _context = context;
    }

    private int GetPsicologoId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim?.Value ?? "0");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
    {
        var psicologoId = GetPsicologoId();
        
        var pacientes = await _context.Pacientes
            .Where(p => p.PsicologoId == psicologoId)
            .OrderBy(p => p.Nome)
            .Select(p => new PacienteDto(p.Id, p.Nome, p.DataCadastro))
            .ToListAsync();

        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteDto>> GetPaciente(int id)
    {
        var psicologoId = GetPsicologoId();
        
        var paciente = await _context.Pacientes
            .Where(p => p.Id == id && p.PsicologoId == psicologoId)
            .Select(p => new PacienteDto(p.Id, p.Nome, p.DataCadastro))
            .FirstOrDefaultAsync();

        if (paciente == null)
            return NotFound();

        return Ok(paciente);
    }

    [HttpPost]
    public async Task<ActionResult<PacienteDto>> CreatePaciente([FromBody] CreatePacienteRequest request)
    {
        var psicologoId = GetPsicologoId();

        var paciente = new Paciente
        {
            Nome = request.Nome,
            PsicologoId = psicologoId,
            DataCadastro = DateTime.UtcNow
        };

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPaciente), 
            new { id = paciente.Id }, 
            new PacienteDto(paciente.Id, paciente.Nome, paciente.DataCadastro)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var psicologoId = GetPsicologoId();
        
        var paciente = await _context.Pacientes
            .FirstOrDefaultAsync(p => p.Id == id && p.PsicologoId == psicologoId);

        if (paciente == null)
            return NotFound();

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
