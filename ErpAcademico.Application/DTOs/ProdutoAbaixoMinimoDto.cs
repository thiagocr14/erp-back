namespace ErpAcademico.Application.DTOs;

public class ProdutoAbaixoMinimoDto
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
}