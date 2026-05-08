namespace ErpAcademico.Application.DTOs;

public class AjusteEstoqueDto
{
    public Guid ProdutoId { get; set; }

    public int NovaQuantidade { get; set; }

    public string Observacao { get; set; }
        = string.Empty;
}