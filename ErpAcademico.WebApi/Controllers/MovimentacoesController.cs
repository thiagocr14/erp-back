using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovimentacoesController
    : ControllerBase
{
    private readonly EstoqueService
        _estoqueService;

    public MovimentacoesController(
        EstoqueService estoqueService)
    {
        _estoqueService =
            estoqueService;
    }

    [HttpGet]
    public async Task<IActionResult>
        ObterMovimentacoes(
            [FromQuery]
            FiltroMovimentacaoDto filtro)
    {
        var movimentacoes =
            await _estoqueService
                .ObterMovimentacoes(
                    filtro);

        return Ok(movimentacoes);
    }
}