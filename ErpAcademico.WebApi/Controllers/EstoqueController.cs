using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstoqueController
    : ControllerBase
{
    private readonly EstoqueService
        _service;

    public EstoqueController(
        EstoqueService service)
    {
        _service = service;
    }

    [HttpPost("entrada")]
    public async Task<IActionResult>
        EntradaEstoque(
            EntradaEstoqueDto dto)
    {
        await _service
            .EntradaEstoque(dto);

        return Ok(new
        {
            mensagem =
                "Entrada realizada com sucesso."
        });
    }

    [HttpPost("ajuste")]
    public async Task<IActionResult>
        AjustarEstoque(
            AjusteEstoqueDto dto)
    {
        await _service
            .AjustarEstoque(dto);

        return Ok(new
        {
            mensagem =
                "Ajuste realizado com sucesso."
        });
    }

    [HttpGet("movimentacoes")]
    public async Task<IActionResult>
        ObterMovimentacoes(
            [FromQuery]
            FiltroMovimentacaoDto filtro)
    {
        var movimentacoes =
            await _service
                .ObterMovimentacoes(filtro);

        return Ok(
            new ResponseDto<
                PaginacaoDto<
                    MovimentacaoEstoqueDto>>
            {
                Sucesso = true,

                Mensagem =
                    "Movimentações obtidas com sucesso.",

                Dados = movimentacoes
            });
    }
}