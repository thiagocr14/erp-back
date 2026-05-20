using ErpAcademico.Application.DTOs;
using ErpAcademico.Domain.Entities;
using ErpAcademico.Domain.Enums;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace ErpAcademico.Infrastructure.Services;
public class ProdutoService
{
    private readonly AppDbContext _context;
    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }
    public async Task AdicionarProduto(ProdutoDto dto)
    {
        var produto = new Produto(
            dto.Nome,
            dto.Descricao,
            dto.PrecoVenda,
            dto.PrecoCusto,
            dto.QuantidadeAtual,
            dto.EstoqueMinimo,
            dto.EstoqueIdeal);
        await _context.Produtos.AddAsync(produto);
        if (dto.QuantidadeAtual > 0)
        {
            var movimentacao = new MovimentacaoEstoque(
                produto.Id,
                TipoMovimentacao.Entrada,
                dto.QuantidadeAtual,
                "Estoque inicial");
            await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        }
        await _context.SaveChangesAsync();
    }
    public async Task<List<Produto>> ObterTodos()
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
    public async Task<IEnumerable<ProdutoDto>> ObterComFiltro(ProdutoFiltroDto filtro)
    {
        var query = _context.Produtos
            .AsNoTracking()
            .Where(p => p.Ativo)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro.Nome))
        {
            query = query.Where(p =>
                EF.Functions.ILike(p.Nome, $"%{filtro.Nome}%"));
        }
        return await query
            .OrderBy(p => p.Nome)
            .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
            .Take(filtro.TamanhoPagina)
            .Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao ?? string.Empty,
                PrecoVenda = p.PrecoVenda,
                PrecoCusto = p.PrecoCusto,
                QuantidadeAtual = p.QuantidadeAtual,
                EstoqueMinimo = p.EstoqueMinimo,
                EstoqueIdeal = p.EstoqueIdeal
            })
            .ToListAsync();
    }
    public async Task DesativarProduto(Guid id)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null)
            throw new Exception("Produto não encontrado.");
        produto.Desativar();
        await _context.SaveChangesAsync();
    }
    public async Task RestaurarProduto(Guid id)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null)
            throw new Exception("Produto não encontrado.");
        produto.Reativar();
        await _context.SaveChangesAsync();
    }
}