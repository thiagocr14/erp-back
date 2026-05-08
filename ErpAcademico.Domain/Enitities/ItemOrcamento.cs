namespace ErpAcademico.Domain.Entities;

public class ItemOrcamento
{
    public Guid Id { get; private set; }

    public Guid OrcamentoId { get; private set; }

    public Orcamento? Orcamento { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Produto? Produto { get; private set; }

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    protected ItemOrcamento() { }

    public ItemOrcamento(
        Guid produtoId,
        int quantidade,
        decimal precoUnitario)
    {
        Id = Guid.NewGuid();

        ProdutoId = produtoId;

        Quantidade = quantidade;

        PrecoUnitario = precoUnitario;
    }
}