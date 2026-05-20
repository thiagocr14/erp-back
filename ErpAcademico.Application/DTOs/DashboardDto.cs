namespace ErpAcademico.Application.DTOs;
public class DashboardDto
{
    public decimal FaturamentoTotal { get; set; }
    public decimal ValorTotalEstoque { get; set; }
    public int QuantidadeProdutos { get; set; }
    public int QuantidadeVendas { get; set; }
    public List<ProdutoAbaixoMinimoDto> ProdutosAbaixoMinimo { get; set; } = new();
    public List<FaturamentoMensalDto> FaturamentoMensal { get; set; } = new();
}

public class FaturamentoMensalDto
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public string MesNome { get; set; } = string.Empty;
    public decimal Total { get; set; }
}