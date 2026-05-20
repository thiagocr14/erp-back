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
                .Where(p => p.Ativo)
                .SumAsync(p => (decimal?)( p.QuantidadeAtual * p.PrecoCusto)) ?? 0;

        var quantidadeProdutos =
            await _context.Produtos
                .AsNoTracking()
                .Where(p => p.Ativo)
                .CountAsync();

        var quantidadeVendas =
            await _context.Vendas
                .AsNoTracking()
                .CountAsync();

        var produtosAbaixoMinimo =
            await _context.Produtos
                .AsNoTracking()
                .Where(p => p.Ativo && p.QuantidadeAtual < p.EstoqueMinimo)
                .Select(p => new ProdutoAbaixoMinimoDto
                {
                    Nome = p.Nome,
                    QuantidadeAtual = p.QuantidadeAtual,
                    EstoqueMinimo = p.EstoqueMinimo
                })
                .ToListAsync();

        var vendas = await _context.Vendas
            .AsNoTracking()
            .ToListAsync();

        var faturamentoMensal = vendas
            .GroupBy(v => new { v.DataVenda.Year, v.DataVenda.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new FaturamentoMensalDto
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                MesNome = new DateTime(g.Key.Year, g.Key.Month, 1)
                    .ToString("MMM/yyyy"),
                Total = g.Sum(v => v.ValorTotal)
            })
            .ToList();

        return new DashboardDto
        {
            FaturamentoTotal = faturamentoTotal,
            ValorTotalEstoque = valorTotalEstoque,
            QuantidadeProdutos = quantidadeProdutos,
            QuantidadeVendas = quantidadeVendas,
            ProdutosAbaixoMinimo = produtosAbaixoMinimo,
            FaturamentoMensal = faturamentoMensal
        };
    }

    public async Task<DashboardDetalhadoDto> ObterDashboardDetalhado()
    {
        var faturamento =
            await _context.Vendas
                .AsNoTracking()
                .SumAsync(v => (decimal?)v.ValorTotal) ?? 0;

        var valorEstoque =
            await _context.Produtos
                .AsNoTracking()
                .Where(p => p.Ativo)
                .SumAsync(p => (decimal?)(p.PrecoCusto * p.QuantidadeAtual)) ?? 0;

        var produtosMaisVendidos =
            await _context.ItensVenda
                .AsNoTracking()
                .Include(i => i.Produto)
                .GroupBy(i => i.Produto!.Nome)
                .OrderByDescending(g => g.Sum(x => x.Quantidade))
                .Take(5)
                .Select(g => $"{g.Key} - {g.Sum(x => x.Quantidade)} vendidos")
                .ToListAsync();

        var produtosMaiorEstoque =
            await _context.Produtos
                .AsNoTracking()
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.QuantidadeAtual)
                .Take(5)
                .Select(p => $"{p.Nome} - {p.QuantidadeAtual} unidades")
                .ToListAsync();

        var ultimasMovimentacoes =
            await _context.MovimentacoesEstoque
                .AsNoTracking()
                .Include(m => m.Produto)
                .OrderByDescending(m => m.DataMovimentacao)
                .Take(10)
                .Select(m => $"{m.Produto.Nome} - {m.Tipo} - {m.Quantidade}")
                .ToListAsync();

        return new DashboardDetalhadoDto
        {
            FaturamentoTotal = faturamento,
            ValorTotalEstoque = valorEstoque,
            QuantidadeProdutos = await _context.Produtos.Where(p => p.Ativo).CountAsync(),
            QuantidadeVendas = await _context.Vendas.CountAsync(),
            ProdutosMaisVendidos = produtosMaisVendidos,
            ProdutosMaiorEstoque = produtosMaiorEstoque,
            UltimasMovimentacoes = ultimasMovimentacoes
        };
    }
}