using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    private int GetPsicologoId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim?.Value ?? "0");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
    {
        var pacientes = await _pacienteService.GetAllByPsicologoAsync(GetPsicologoId());
        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteDto>> GetPaciente(int id)
    {
        var paciente = await _pacienteService.GetByIdAsync(id, GetPsicologoId());

        if (paciente == null)
            return NotFound();

        return Ok(paciente);
    }

    [HttpPost]
    public async Task<ActionResult<PacienteDto>> CreatePaciente([FromBody] CreatePacienteRequest request)
    {
        var paciente = await _pacienteService.CreateAsync(request, GetPsicologoId());

        return CreatedAtAction(
            nameof(GetPaciente), 
            new { id = paciente.Id }, 
            paciente
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var success = await _pacienteService.DeleteAsync(id, GetPsicologoId());

        if (!success)
            return NotFound();

        return NoContent();
    }
}
