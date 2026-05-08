using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrcamentosController : ControllerBase
{
    private readonly OrcamentoService _orcamentoService;

    public OrcamentosController(
        OrcamentoService orcamentoService)
    {
        _orcamentoService = orcamentoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarOrcamento(
        [FromBody] CriarOrcamentoDto dto)
    {
        var orcamentoId =
            await _orcamentoService
                .CriarOrcamento(dto);

        return Ok(new
        {
            mensagem = "Orçamento criado com sucesso.",
            orcamentoId
        });
    }


[HttpPost("{id}/converter")]
public async Task<IActionResult> ConverterEmVenda(
    Guid id)
{
    var vendaId =
        await _orcamentoService
            .ConverterEmVenda(id);

    return Ok(new
    {
        mensagem =
            "Orçamento convertido em venda com sucesso.",
        vendaId
    });
}
}