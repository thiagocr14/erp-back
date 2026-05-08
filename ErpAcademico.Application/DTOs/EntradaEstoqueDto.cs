namespace ErpAcademico.Application.DTOs;

public class EntradaEstoqueDto
{
    public Guid ProdutoId { get; set; }

    public int Quantidade { get; set; }

    public string Observacao { get; set; }
        = string.Empty;
}