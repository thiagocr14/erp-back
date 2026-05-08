using ErpAcademico.Domain.Enums;

namespace ErpAcademico.Domain.Entities;

public class MovimentacaoEstoque
{
    public Guid Id { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Produto Produto { get; private set; } = null!;

    public TipoMovimentacao Tipo { get; private set; }

    public int Quantidade { get; private set; }

    public string Observacao { get; private set; }
        = string.Empty;

    public DateTime DataMovimentacao
    {
        get;
        private set;
    }

    protected MovimentacaoEstoque() { }

    public MovimentacaoEstoque(
        Guid produtoId,
        TipoMovimentacao tipo,
        int quantidade,
        string observacao)
    {
        Id = Guid.NewGuid();

        ProdutoId = produtoId;

        Tipo = tipo;

        Quantidade = quantidade;

        Observacao = observacao;

        DataMovimentacao = DateTime.UtcNow;
    }
}