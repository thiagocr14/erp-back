using ErpAcademico.Application.DTOs;
using ErpAcademico.Domain.Entities;
using ErpAcademico.Domain.Enums;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ErpAcademico.Infrastructure.Services;

public class OrcamentoService
{
    private readonly AppDbContext _context;

    public OrcamentoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CriarOrcamento(
        CriarOrcamentoDto dto)
    {
        decimal valorTotal = 0;

        var itens = new List<ItemOrcamento>();

        foreach (var itemDto in dto.Itens)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p =>
                    p.Id == itemDto.ProdutoId);

            if (produto is null)
                throw new Exception(
                    "Produto não encontrado.");

            var item = new ItemOrcamento(
                produto.Id,
                itemDto.Quantidade,
                produto.PrecoVenda);

            itens.Add(item);

            valorTotal +=
                produto.PrecoVenda *
                itemDto.Quantidade;
        }

        var orcamento = new Orcamento(valorTotal);

        foreach (var item in itens)
        {
            orcamento.Itens.Add(item);
        }

        await _context.Orcamentos.AddAsync(orcamento);

        await _context.SaveChangesAsync();

        return orcamento.Id;
    }

    public async Task<Guid> ConverterEmVenda(
        Guid orcamentoId)
    {
        using var transaction =
            await _context.Database
                .BeginTransactionAsync();

        try
        {
            var orcamento = await _context.Orcamentos
                .Include(o => o.Itens)
                .FirstOrDefaultAsync(o =>
                    o.Id == orcamentoId);

            if (orcamento is null)
                throw new Exception(
                    "Orçamento não encontrado.");

            if (orcamento.Status !=
                StatusOrcamento.Pendente)
            {
                throw new Exception(
                    "Somente orçamentos pendentes podem ser convertidos.");
            }

            decimal valorTotal = 0;

            var itensVenda = new List<ItemVenda>();

            foreach (var item in orcamento.Itens)
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p =>
                        p.Id == item.ProdutoId);

                if (produto is null)
                    throw new Exception(
                        "Produto não encontrado.");

                if (produto.QuantidadeAtual <
                    item.Quantidade)
                {
                    throw new Exception(
                        $"Estoque insuficiente para o produto {produto.Nome}");
                }

                produto.BaixarEstoque(
                    item.Quantidade);

                var itemVenda = new ItemVenda(
                    produto.Id,
                    item.Quantidade,
                    item.PrecoUnitario);

                itensVenda.Add(itemVenda);

                valorTotal +=
                    item.PrecoUnitario *
                    item.Quantidade;
            }

            var venda = new Venda(valorTotal);

            foreach (var itemVenda in itensVenda)
            {
                venda.Itens.Add(itemVenda);
            }

            orcamento.Aprovar();

            await _context.Vendas.AddAsync(venda);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return venda.Id;
        }
        catch
        {
            await transaction.RollbackAsync();

            throw;
        }
    }
}