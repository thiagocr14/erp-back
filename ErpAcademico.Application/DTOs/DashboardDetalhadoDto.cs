namespace ErpAcademico.Application.DTOs;

public class DashboardDetalhadoDto
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

    public List<string>
        ProdutosMaisVendidos
    {
        get;
        set;
    } = new();

    public List<string>
        ProdutosMaiorEstoque
    {
        get;
        set;
    } = new();

    public List<string>
        UltimasMovimentacoes
    {
        get;
        set;
    } = new();
}