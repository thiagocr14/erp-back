using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendasController : ControllerBase
{
    private readonly VendaService _vendaService;

    public VendasController(VendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpPost]
    public async Task<IActionResult> RealizarVenda(
        [FromBody] RealizarVendaDto dto)
    {
        var vendaId =
            await _vendaService.RealizarVenda(dto);

        return Ok(new
        {
            mensagem = "Venda realizada com sucesso.",
            vendaId
        });
    }
}