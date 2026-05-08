namespace ErpAcademico.Application.DTOs;

public class DashboardDto
{
    public decimal FaturamentoTotal
    {
        get;
        set;
    }

    public decimal ValorTotalEstoque
    {
        get;
        set;
    }

    public int QuantidadeProdutos
    {
        get;
        set;
    }

    public int QuantidadeVendas
    {
        get;
        set;
    }

    public List<ProdutoAbaixoMinimoDto>
        ProdutosAbaixoMinimo
    {
        get;
        set;
    } = new();
}