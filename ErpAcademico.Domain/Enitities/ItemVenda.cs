namespace ErpAcademico.Domain.Entities;

public class ItemVenda
{
    public Guid Id { get; private set; }

    public Guid VendaId { get; private set; }

    public Venda? Venda { get; private set; }

    public Guid ProdutoId { get; private set; }

    public Produto? Produto { get; private set; }

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; private set; }

    protected ItemVenda() { }

    public ItemVenda(
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