namespace ErpAcademico.Application.DTOs;

public class FiltroMovimentacaoDto
{
    public string? Produto
    {
        get;
        set;
    }

    public string? Tipo
    {
        get;
        set;
    }

    public int Pagina
    {
        get;
        set;
    } = 1;

    public int TamanhoPagina
    {
        get;
        set;
    } = 10;
}