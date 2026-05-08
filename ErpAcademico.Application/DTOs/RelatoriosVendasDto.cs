namespace ErpAcademico.Application.DTOs;

public class RelatorioVendasDto
{
    public decimal TotalFaturado
    {
        get;
        set;
    }

    public int QuantidadeVendas
    {
        get;
        set;
    }

    public decimal TicketMedio
    {
        get;
        set;
    }

    public List<VendaResumoDto>
        Vendas
    {
        get;
        set;
    } = new();
}