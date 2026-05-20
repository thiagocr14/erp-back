using ErpAcademico.Application.DTOs;
using ErpAcademico.Domain.Entities;
using ErpAcademico.Domain.Enums;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ErpAcademico.Domain.Exceptions;
namespace ErpAcademico.Infrastructure.Services;
public class EstoqueService
{
    private readonly AppDbContext _context;
    public EstoqueService(AppDbContext context)
    {
        _context = context;
    }
    public async Task EntradaEstoque(EntradaEstoqueDto dto)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == dto.ProdutoId);
        if (produto is null)
            throw new NaoEncontradoException("Produto não encontrado.");
        produto.AdicionarEstoque(dto.Quantidade);
        var movimentacao = new MovimentacaoEstoque(
            produto.Id,
            TipoMovimentacao.Entrada,
            dto.Quantidade,
            dto.Observacao);
        await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }
    public async Task AjustarEstoque(AjusteEstoqueDto dto)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == dto.ProdutoId);
        if (produto is null)
            throw new NaoEncontradoException("Produto não encontrado.");
        var diferenca = dto.NovaQuantidade - produto.QuantidadeAtual;
        produto.AjustarEstoque(dto.NovaQuantidade);
        var movimentacao = new MovimentacaoEstoque(
            produto.Id,
            TipoMovimentacao.Ajuste,
            diferenca,
            dto.Observacao);
        await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }
    public async Task<PaginacaoDto<MovimentacaoEstoqueDto>> ObterMovimentacoes(
        FiltroMovimentacaoDto filtro)
    {
        var query = _context.MovimentacoesEstoque
            .AsNoTracking()
            .Include(m => m.Produto)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Produto))
        {
            query = query.Where(m =>
                EF.Functions.ILike(m.Produto.Nome, $"%{filtro.Produto}%"));
        }
        if (!string.IsNullOrWhiteSpace(filtro.Tipo))
        {
            if (Enum.TryParse<TipoMovimentacao>(filtro.Tipo, true, out var tipoEnum))
                query = query.Where(m => m.Tipo == tipoEnum);
        }
        if (filtro.DataInicial.HasValue)
        {
            var dataInicial = filtro.DataInicial.Value.Date.ToUniversalTime();
            query = query.Where(m => m.DataMovimentacao >= dataInicial);
        }
        if (filtro.DataFinal.HasValue)
        {
            var dataFinal = filtro.DataFinal.Value.Date.AddDays(1).ToUniversalTime();
            query = query.Where(m => m.DataMovimentacao < dataFinal);
        }

        var totalRegistros = await query.CountAsync();
        var movimentacoes = await query
            .OrderByDescending(m => m.DataMovimentacao)
            .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
            .Take(filtro.TamanhoPagina)
            .ToListAsync();

        return new PaginacaoDto<MovimentacaoEstoqueDto>
        {
            Pagina = filtro.Pagina,
            TamanhoPagina = filtro.TamanhoPagina,
            TotalRegistros = totalRegistros,
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtro.TamanhoPagina),
            Itens = movimentacoes.Select(m => new MovimentacaoEstoqueDto
            {
                Id = m.Id,
                Produto = m.Produto.Nome,
                Tipo = Enum.GetName(typeof(TipoMovimentacao), m.Tipo) ?? "Desconhecido",
                Quantidade = m.Quantidade,
                Observacao = m.Observacao,
                DataMovimentacao = m.DataMovimentacao
            }).ToList()
        };
    }
}