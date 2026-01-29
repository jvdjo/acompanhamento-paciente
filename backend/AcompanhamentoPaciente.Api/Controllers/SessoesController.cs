using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcompanhamentoPaciente.Api.Data;
using AcompanhamentoPaciente.Api.DTOs;
using AcompanhamentoPaciente.Api.Models;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/pacientes/{pacienteId}/[controller]")]
[Authorize]
public class SessoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SessoesController(AppDbContext context)
    {
        _context = context;
    }

    private int GetPsicologoId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim?.Value ?? "0");
    }

    private async Task<bool> PacienteBelongsToPsicologo(int pacienteId)
    {
        var psicologoId = GetPsicologoId();
        return await _context.Pacientes
            .AnyAsync(p => p.Id == pacienteId && p.PsicologoId == psicologoId);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessaoDto>>> GetSessoes(int pacienteId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId))
            return NotFound();

        var sessoes = await _context.Sessoes
            .Where(s => s.PacienteId == pacienteId)
            .OrderByDescending(s => s.Data)
            .Select(s => new SessaoDto(s.Id, s.Data, s.Anotacoes))
            .ToListAsync();

        return Ok(sessoes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SessaoDto>> GetSessao(int pacienteId, int id)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId))
            return NotFound();

        var sessao = await _context.Sessoes
            .Where(s => s.Id == id && s.PacienteId == pacienteId)
            .Select(s => new SessaoDto(s.Id, s.Data, s.Anotacoes))
            .FirstOrDefaultAsync();

        if (sessao == null)
            return NotFound();

        return Ok(sessao);
    }

    [HttpPost]
    public async Task<ActionResult<SessaoDto>> CreateSessao(int pacienteId, [FromBody] CreateSessaoRequest request)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId))
            return NotFound();

        var sessao = new Sessao
        {
            PacienteId = pacienteId,
            Data = request.Data ?? DateTime.UtcNow
        };

        _context.Sessoes.Add(sessao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSessao), 
            new { pacienteId, id = sessao.Id }, 
            new SessaoDto(sessao.Id, sessao.Data, sessao.Anotacoes)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSessao(int pacienteId, int id, [FromBody] UpdateSessaoRequest request)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId))
            return NotFound();

        var sessao = await _context.Sessoes
            .FirstOrDefaultAsync(s => s.Id == id && s.PacienteId == pacienteId);

        if (sessao == null)
            return NotFound();

        sessao.Anotacoes = request.Anotacoes;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSessao(int pacienteId, int id)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId))
            return NotFound();

        var sessao = await _context.Sessoes
            .FirstOrDefaultAsync(s => s.Id == id && s.PacienteId == pacienteId);

        if (sessao == null)
            return NotFound();

        _context.Sessoes.Remove(sessao);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
