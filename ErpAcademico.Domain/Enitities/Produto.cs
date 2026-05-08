namespace ErpAcademico.Domain.Entities;

public class Produto
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; } = string.Empty;

    public string? Descricao { get; private set; }

    public decimal PrecoVenda { get; private set; }

    public decimal PrecoCusto { get; private set; }

    public int QuantidadeAtual { get; private set; }

    public int EstoqueMinimo { get; private set; }

    public int EstoqueIdeal { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // Propriedade calculada
    public decimal ValorTotalEmEstoque =>
        QuantidadeAtual * PrecoCusto;

    // Indicador de alerta
    public bool AbaixoDoEstoqueMinimo =>
        QuantidadeAtual < EstoqueMinimo;

    public ICollection<MovimentacaoEstoque>
    Movimentacoes { get; private set; }
    = new List<MovimentacaoEstoque>();

    protected Produto() { }

    public Produto(
        string nome,
        string? descricao,
        decimal precoVenda,
        decimal precoCusto,
        int quantidadeAtual,
        int estoqueMinimo,
        int estoqueIdeal)
    {
        Id = Guid.NewGuid();

        Nome = nome;
        Descricao = descricao;

        PrecoVenda = precoVenda;
        PrecoCusto = precoCusto;

        QuantidadeAtual = quantidadeAtual;

        EstoqueMinimo = estoqueMinimo;
        EstoqueIdeal = estoqueIdeal;

        CreatedAt = DateTime.UtcNow;
    }

    public void DebitarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        if (QuantidadeAtual < quantidade)
            throw new InvalidOperationException("Estoque insuficiente.");

        QuantidadeAtual -= quantidade;
    }

    public void AdicionarEstoque(
    int quantidade)
{
    if (quantidade <= 0)
        throw new Exception(
            "Quantidade inválida.");

    QuantidadeAtual += quantidade;
}

    public void ReporEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        QuantidadeAtual += quantidade;
    }

    public void AtualizarPrecoVenda(decimal precoVenda)
    {
        if (precoVenda < 0)
            throw new ArgumentException("Preço inválido.");

        PrecoVenda = precoVenda;
    }

    public void AtualizarPrecoCusto(decimal precoCusto)
    {
        if (precoCusto < 0)
            throw new ArgumentException("Preço inválido.");

        PrecoCusto = precoCusto;
    }

    public void AjustarEstoque(
    int novaQuantidade)
{
    if (novaQuantidade < 0)
        throw new Exception(
            "Quantidade inválida.");

    QuantidadeAtual = novaQuantidade;
}
    public void BaixarEstoque(int quantidade)
{
    if (quantidade <= 0)
        throw new Exception(
            "Quantidade inválida.");

    if (QuantidadeAtual < quantidade)
        throw new Exception(
            "Estoque insuficiente.");
            

    QuantidadeAtual -= quantidade;
}
}