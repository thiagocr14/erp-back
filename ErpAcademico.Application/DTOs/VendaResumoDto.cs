namespace ErpAcademico.Application.DTOs;

public class VendaResumoDto
{
    public Guid Id { get; set; }

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