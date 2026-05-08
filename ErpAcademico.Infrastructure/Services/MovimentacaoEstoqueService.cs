using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ErpAcademico.Infrastructure.Services;

public class MovimentacaoEstoqueService
{
    private readonly AppDbContext _context;

    public MovimentacaoEstoqueService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<
        List<MovimentacaoEstoqueDto>>
        ObterTodas()
    {
        return await _context
            .MovimentacoesEstoque
            .Include(m => m.Produto)
            .Select(m =>
                new MovimentacaoEstoqueDto
                {
                    Id = m.Id,

                    Produto = m.Produto.Nome,

                    Tipo = m.Tipo.ToString(),

                    Quantidade = m.Quantidade,

                    Observacao =
                        m.Observacao,

                    DataMovimentacao =
                        m.DataMovimentacao
                })
            .ToListAsync();
    }
}