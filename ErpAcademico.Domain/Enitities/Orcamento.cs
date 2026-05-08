using ErpAcademico.Domain.Enums;

namespace ErpAcademico.Domain.Entities;

public class Orcamento
{
    public Guid Id { get; private set; }

    public DateTime DataCriacao { get; private set; }

    public decimal ValorTotal { get; private set; }

    public StatusOrcamento Status { get; private set; }

    public ICollection<ItemOrcamento> Itens
    { get; private set; }
        = new List<ItemOrcamento>();

    protected Orcamento() { }

    public Orcamento(decimal valorTotal)
    {
        Id = Guid.NewGuid();

        DataCriacao = DateTime.UtcNow;

        ValorTotal = valorTotal;

        Status = StatusOrcamento.Pendente;
    }

    public void Aprovar()
    {
        Status = StatusOrcamento.Aprovado;
    }

    public void Cancelar()
    {
        Status = StatusOrcamento.Cancelado;
    }
}