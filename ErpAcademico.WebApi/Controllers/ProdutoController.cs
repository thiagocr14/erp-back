using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController
    : ControllerBase
{
    private readonly ProdutoService
        _produtoService;

    public ProdutosController(
        ProdutoService produtoService)
    {
        _produtoService =
            produtoService;
    }

    [HttpPost]
    public async Task<IActionResult>
        AdicionarProduto(
            ProdutoDto dto)
    {
        await _produtoService
            .AdicionarProduto(dto);

        return Ok(
            "Produto cadastrado.");
    }

    [HttpGet]
    public async Task<IActionResult>
        ObterTodos()
    {
        var produtos =
            await _produtoService
                .ObterTodos();

        return Ok(produtos);
    }

    [HttpGet("filtro")]
    public async Task<IActionResult>
        ObterComFiltro(
            [FromQuery]
            ProdutoFiltroDto filtro)
    {
        var produtos =
            await _produtoService
                .ObterComFiltro(filtro);

        return Ok(produtos);
    }
}