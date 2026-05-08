namespace ErpAcademico.Domain.Entities;
using ErpAcademico.Domain.Enums;
public class Venda
{
    public Guid Id { get; private set; }
    public DateTime DataVenda { get; private set; }
    public decimal ValorTotal { get; private set; }
    
    // ADICIONE ESTA LINHA AQUI:
    public StatusVenda Status { get; private set; }

    public ICollection<ItemVenda> Itens { get; private set; } = new List<ItemVenda>();

    protected Venda() { }

    public Venda(decimal valorTotal)
    {
        Id = Guid.NewGuid();
        DataVenda = DateTime.UtcNow;
        ValorTotal = valorTotal;
        Status = StatusVenda.Finalizada;
    }
}