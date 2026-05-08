using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatoriosController
    : ControllerBase
{
    private readonly RelatorioService
        _service;

    public RelatoriosController(
        RelatorioService service)
    {
        _service = service;
    }

    [HttpGet("vendas")]
    public async Task<IActionResult>
        ObterRelatorioVendas(
            DateTime? dataInicial,
            DateTime? dataFinal)
    {
        var relatorio =
            await _service
                .ObterRelatorioVendas(
                    dataInicial,
                    dataFinal);

        return Ok(relatorio);
    }
        [HttpGet("fechamento-dia")]
public async Task<IActionResult>
    ObterFechamentoDia(
        [FromQuery]
        DateTime? data)
{
    var relatorio =
        await _service
            .ObterFechamentoDia(
                data);

    return Ok(relatorio);
}
    }
