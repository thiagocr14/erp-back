namespace ErpAcademico.Application.DTOs;

public class ProdutoDto
{
    public Guid Id
    {
        get;
        set;
    }

    public string Nome
    {
        get;
        set;
    } = string.Empty;

    public string Descricao
    {
        get;
        set;
    } = string.Empty;

    public decimal PrecoVenda
    {
        get;
        set;
    }

    public decimal PrecoCusto
    {
        get;
        set;
    }

    public int QuantidadeAtual
    {
        get;
        set;
    }

    public int EstoqueMinimo
    {
        get;
        set;
    }

    public int EstoqueIdeal
    {
        get;
        set;
    }
}