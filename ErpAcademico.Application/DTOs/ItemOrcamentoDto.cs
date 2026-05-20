namespace ErpAcademico.Application.DTOs;

public class ItemOrcamentoDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    
    // Adicione as propriedades abaixo que estavam faltando:
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
}