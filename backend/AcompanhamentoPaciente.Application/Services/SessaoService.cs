using AcompanhamentoPaciente.Application.DTOs;
using AcompanhamentoPaciente.Application.Interfaces;
using AcompanhamentoPaciente.Domain.Entities;
using AcompanhamentoPaciente.Domain.Interfaces;

namespace AcompanhamentoPaciente.Application.Services;

public class SessaoService : ISessaoService
{
    private readonly ISessaoRepository _sessaoRepository;
    private readonly IPacienteRepository _pacienteRepository;

    public SessaoService(ISessaoRepository sessaoRepository, IPacienteRepository pacienteRepository)
    {
        _sessaoRepository = sessaoRepository;
        _pacienteRepository = pacienteRepository;
    }

    private async Task<bool> PacienteBelongsToPsicologo(int pacienteId, int psicologoId)
    {
        var paciente = await _pacienteRepository.GetByIdAndPsicologoIdAsync(pacienteId, psicologoId);
        return paciente != null;
    }

    public async Task<IEnumerable<SessaoDto>> GetAllByPacienteAsync(int pacienteId, int psicologoId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId, psicologoId))
            return Enumerable.Empty<SessaoDto>();

        var sessoes = await _sessaoRepository.GetByPacienteIdAsync(pacienteId);
        return sessoes
            .OrderByDescending(s => s.Data)
            .Select(s => new SessaoDto(s.Id, s.Data, s.Anotacoes));
    }

    public async Task<SessaoDto?> GetByIdAsync(int id, int pacienteId, int psicologoId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId, psicologoId))
            return null;

        var sessao = await _sessaoRepository.GetByIdAndPacienteIdAsync(id, pacienteId);
        if (sessao == null) return null;
        
        return new SessaoDto(sessao.Id, sessao.Data, sessao.Anotacoes);
    }

    public async Task<SessaoDto?> CreateAsync(CreateSessaoRequest request, int pacienteId, int psicologoId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId, psicologoId))
            return null;

        var sessao = new Sessao
        {
            PacienteId = pacienteId,
            Data = request.Data ?? DateTime.UtcNow
        };

        await _sessaoRepository.AddAsync(sessao);
        
        return new SessaoDto(sessao.Id, sessao.Data, sessao.Anotacoes);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSessaoRequest request, int pacienteId, int psicologoId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId, psicologoId))
            return false;

        var sessao = await _sessaoRepository.GetByIdAndPacienteIdAsync(id, pacienteId);
        if (sessao == null) return false;

        sessao.Anotacoes = request.Anotacoes;
        await _sessaoRepository.UpdateAsync(sessao);
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id, int pacienteId, int psicologoId)
    {
        if (!await PacienteBelongsToPsicologo(pacienteId, psicologoId))
            return false;

        var sessao = await _sessaoRepository.GetByIdAndPacienteIdAsync(id, pacienteId);
        if (sessao == null) return false;

        await _sessaoRepository.DeleteAsync(sessao);
        return true;
    }
}
