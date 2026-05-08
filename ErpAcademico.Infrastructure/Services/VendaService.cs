using ErpAcademico.Application.DTOs;
using ErpAcademico.Domain.Entities;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ErpAcademico.Domain.Enums;

namespace ErpAcademico.Infrastructure.Services;

public class VendaService
{
    private readonly AppDbContext _context;

    public VendaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> RealizarVenda(
        RealizarVendaDto dto)
    {
        using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            decimal valorTotal = 0;

            var itensVenda = new List<ItemVenda>();

            foreach (var itemDto in dto.Itens)
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p =>
                        p.Id == itemDto.ProdutoId);

                if (produto is null)
                    throw new Exception(
                        "Produto não encontrado.");

                if (produto.QuantidadeAtual <
                    itemDto.Quantidade)
                {
                    throw new Exception(
                        $"Estoque insuficiente para o produto {produto.Nome}");
                }
                

                
                produto.DebitarEstoque(
                    itemDto.Quantidade);
                
                var movimentacao =
    new MovimentacaoEstoque(
        produto.Id,
        TipoMovimentacao.Saida,
        itemDto.Quantidade,
        $"Venda realizada - Produto: {produto.Nome}");
        
                await _context.MovimentacoesEstoque
                .AddAsync(movimentacao);

                var itemVenda = new ItemVenda(
                    produto.Id,
                    itemDto.Quantidade,
                    produto.PrecoVenda);

                itensVenda.Add(itemVenda);

                valorTotal +=
                    produto.PrecoVenda *
                    itemDto.Quantidade;
            }

            var venda = new Venda(valorTotal);

            foreach (var item in itensVenda)
            {
                venda.Itens.Add(item);
            }

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