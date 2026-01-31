using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;

namespace AcompanhamentoPaciente.Api.Controllers;

[ApiController]
[Route("api/pacientes/{pacienteId}/[controller]")]
[Authorize]
public class SessoesController : ControllerBase
{
    private readonly ISessaoService _sessaoService;

    public SessoesController(ISessaoService sessaoService)
    {
        _sessaoService = sessaoService;
    }

    private Guid GetPsicologoId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim?.Value ?? Guid.Empty.ToString());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessaoDto>>> GetSessoes(Guid pacienteId)
    {
        var sessoes = await _sessaoService.GetAllByPacienteAsync(pacienteId, GetPsicologoId());
        return Ok(sessoes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SessaoDto>> GetSessao(Guid pacienteId, Guid id)
    {
        var sessao = await _sessaoService.GetByIdAsync(id, pacienteId, GetPsicologoId());

        if (sessao == null)
            return NotFound();

        return Ok(sessao);
    }

    [HttpPost]
    public async Task<ActionResult<SessaoDto>> CreateSessao(Guid pacienteId, [FromBody] CreateSessaoRequest request)
    {
        var sessao = await _sessaoService.CreateAsync(request, pacienteId, GetPsicologoId());
        
        if (sessao == null) 
            return NotFound(); // Ou BadRequest dependendo da regra

        return CreatedAtAction(
            nameof(GetSessao), 
            new { pacienteId, id = sessao.Id }, 
            sessao
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSessao(Guid pacienteId, Guid id, [FromBody] UpdateSessaoRequest request)
    {
        var success = await _sessaoService.UpdateAsync(id, request, pacienteId, GetPsicologoId());

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSessao(Guid pacienteId, Guid id)
    {
        var success = await _sessaoService.DeleteAsync(id, pacienteId, GetPsicologoId());

        if (!success)
            return NotFound();

        return NoContent();
    }
}
