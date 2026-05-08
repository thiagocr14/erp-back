using ErpAcademico.Application.DTOs;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ErpAcademico.Infrastructure.Services;

public class RelatorioService
{
    private readonly AppDbContext _context;

    public RelatorioService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<
        RelatorioVendasDto>
        ObterRelatorioVendas(
            DateTime? dataInicial,
            DateTime? dataFinal)
    {
        var query =
            _context.Vendas.AsQueryable();

        if (dataInicial.HasValue)
        {
            query = query.Where(v =>
                v.DataVenda >=
                dataInicial.Value);
        }

        if (dataFinal.HasValue)
        {
            query = query.Where(v =>
                v.DataVenda <=
                dataFinal.Value);
        }

        var vendas =
            await query
                .OrderByDescending(v =>
                    v.DataVenda)
                .Select(v =>
                    new VendaResumoDto
                    {
                        Id = v.Id,

                        DataVenda =
                            v.DataVenda,

                        ValorTotal =
                            v.ValorTotal
                    })
                .ToListAsync();

        var totalFaturado =
            vendas.Sum(v =>
                v.ValorTotal);

        var quantidadeVendas =
            vendas.Count;

        var ticketMedio =
            quantidadeVendas == 0
            ? 0
            : totalFaturado /
              quantidadeVendas;

        return new RelatorioVendasDto
        {
            TotalFaturado =
                totalFaturado,

            QuantidadeVendas =
                quantidadeVendas,

            TicketMedio =
                ticketMedio,

            Vendas = vendas
        };}

        public async Task<FechamentoDiaDto>
    ObterFechamentoDia(
        DateTime? data = null)
{
    var dataConsulta =
        (data ?? DateTime.UtcNow)
        .Date;

    var vendasQuery =
        _context.Vendas
            .AsNoTracking()
            .Where(v =>
                v.DataVenda.Date ==
                dataConsulta);

    var totalFaturado =
        await vendasQuery
            .SumAsync(v =>
                (decimal?)v.ValorTotal)
        ?? 0;

    var totalVendas =
        await vendasQuery
            .CountAsync();

    var vendas =
        await vendasQuery
            .OrderByDescending(v =>
                v.DataVenda)
            .Select(v =>
                new ItemFechamentoDto
                {
                    VendaId = v.Id,

                    DataVenda =
                        v.DataVenda,

                    ValorTotal =
                        v.ValorTotal
                })
            .ToListAsync();

    return new FechamentoDiaDto
    {
        Data = dataConsulta,

        TotalVendas =
            totalVendas,

        TotalFaturado =
            totalFaturado,

        TicketMedio =
            totalVendas > 0
            ? Math.Round(
                totalFaturado /
                totalVendas,
                2)
            : 0,

        Vendas = vendas
    };
}
    }
