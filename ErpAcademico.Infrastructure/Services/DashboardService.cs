using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace ErpAcademico.Infrastructure.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> ObterDashboard()
    {
        var faturamentoTotal =
            await _context.Vendas
                .AsNoTracking()
                .SumAsync(v => (decimal?)v.ValorTotal) ?? 0;

        var valorTotalEstoque =
            await _context.Produtos
                .AsNoTracking()
                .SumAsync(p =>
                    p.QuantidadeAtual * p.PrecoCusto);

        var quantidadeProdutos =
            await _context.Produtos
                .AsNoTracking()
                .CountAsync();

        var quantidadeVendas =
            await _context.Vendas
                .AsNoTracking()
                .CountAsync();

        var produtosAbaixoMinimo =
            await _context.Produtos
                .AsNoTracking()
                .Where(p =>
                    p.QuantidadeAtual < p.EstoqueMinimo)
                .Select(p =>
                    new ProdutoAbaixoMinimoDto
                    {
                        Nome = p.Nome,
                        QuantidadeAtual = p.QuantidadeAtual,
                        EstoqueMinimo = p.EstoqueMinimo
                    })
                .ToListAsync();

        return new DashboardDto
        {
            FaturamentoTotal = faturamentoTotal,
            ValorTotalEstoque = valorTotalEstoque,
            QuantidadeProdutos = quantidadeProdutos,
            QuantidadeVendas = quantidadeVendas,
            ProdutosAbaixoMinimo = produtosAbaixoMinimo
        };}
    
    public async Task<
    DashboardDetalhadoDto>
    ObterDashboardDetalhado()
{
    var faturamento =
        await _context.Vendas
            .SumAsync(v =>
                v.ValorTotal);

    var valorEstoque =
        await _context.Produtos
            .SumAsync(p =>
                p.PrecoCusto *
                p.QuantidadeAtual);

    var produtosMaisVendidos =
    await _context.ItensVenda
        .Include(i => i.Produto)
        .GroupBy(i => i.Produto!.Nome)
        .OrderByDescending(g =>
            g.Sum(x => x.Quantidade))
        .Take(5)
        .Select(g =>
            $"{g.Key} - {g.Sum(x => x.Quantidade)} vendidos")
        .ToListAsync();

    var produtosMaiorEstoque =
        await _context.Produtos
            .OrderByDescending(p =>
                p.QuantidadeAtual)
            .Take(5)
            .Select(p =>
                $"{p.Nome} - {p.QuantidadeAtual} unidades")
            .ToListAsync();

    var ultimasMovimentacoes =
        await _context
            .MovimentacoesEstoque
            .Include(m => m.Produto)
            .OrderByDescending(m =>
                m.DataMovimentacao)
            .Take(10)
            .Select(m =>
                $"{m.Produto.Nome} - {m.Tipo} - {m.Quantidade}")
            .ToListAsync();

    return new DashboardDetalhadoDto
    {
        FaturamentoTotal =
            faturamento,

        ValorTotalEstoque =
            valorEstoque,

        QuantidadeProdutos =
            await _context.Produtos
                .CountAsync(),

        QuantidadeVendas =
            await _context.Vendas
                .CountAsync(),

        ProdutosMaisVendidos =
            produtosMaisVendidos,

        ProdutosMaiorEstoque =
            produtosMaiorEstoque,

        UltimasMovimentacoes =
            ultimasMovimentacoes
    };
}

    }
