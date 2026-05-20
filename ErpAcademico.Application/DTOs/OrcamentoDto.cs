namespace ErpAcademico.Application.DTOs;
public class OrcamentoDto
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ItemOrcamentoDto> Itens { get; set; } = new();
}