using ErpAcademico.Domain.Exceptions;

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

    public bool Ativo { get; private set; }

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
        Ativo = true;
    }
public void Desativar()
{
    Ativo = false;
}

public void Reativar()
{
    Ativo = true;
}
    public void DebitarEstoque(
    int quantidade)
{
    if (quantidade <= 0)
        throw new NegocioException(
            "Quantidade inválida.");

    if (QuantidadeAtual < quantidade)
        throw new NegocioException(
            "Estoque insuficiente.");

    QuantidadeAtual -= quantidade;
}

    public void AdicionarEstoque(
    int quantidade)
{
    if (quantidade <= 0)
        throw new NegocioException(
            "Quantidade inválida.");

    QuantidadeAtual += quantidade;
}

    public void ReporEstoque(
    int quantidade)
{
    if (quantidade <= 0)
        throw new NegocioException(
            "Quantidade inválida.");

    QuantidadeAtual += quantidade;
}

    public void AtualizarPrecoVenda(decimal precoVenda)
    {
        if (precoVenda < 0)
            throw new NegocioException(
    "Preço inválido.");
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
       throw new NegocioException(
            "Quantidade inválida.");

    QuantidadeAtual = novaQuantidade;
}
    public void BaixarEstoque(
    int quantidade)
{
    if (quantidade <= 0)
        throw new NegocioException(
            "Quantidade inválida.");

    if (QuantidadeAtual < quantidade)
        throw new NegocioException(
            "Estoque insuficiente.");

    QuantidadeAtual -= quantidade;
}
}