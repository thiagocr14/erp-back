namespace ErpAcademico.Application.DTOs;

public class FechamentoDiaDto
{
    public DateTime Data
    {
        get;
        set;
    }

    public int TotalVendas
    {
        get;
        set;
    }

    public decimal TotalFaturado
    {
        get;
        set;
    }

    public decimal TicketMedio
    {
        get;
        set;
    }

    public List<ItemFechamentoDto>
        Vendas
    {
        get;
        set;
    } = new();
}

public class ItemFechamentoDto
{
    public Guid VendaId
    {
        get;
        set;
    }

    public DateTime DataVenda
    {
        get;
        set;
    }

    public decimal ValorTotal
    {
        get;
        set;
    }
}