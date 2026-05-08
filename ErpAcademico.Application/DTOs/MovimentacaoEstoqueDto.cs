namespace ErpAcademico.Application.DTOs;

public class MovimentacaoEstoqueDto
{
    public Guid Id { get; set; }

    public string Produto { get; set; }
        = string.Empty;

    public string Tipo { get; set; }
        = string.Empty;

    public int Quantidade { get; set; }

    public string Observacao { get; set; }
        = string.Empty;

    public DateTime DataMovimentacao
    {
        get;
        set;
    }
}