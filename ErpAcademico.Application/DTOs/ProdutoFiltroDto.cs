namespace ErpAcademico.Application.DTOs;

public class ProdutoFiltroDto
{
    public string? Nome
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

    public int ObterTamanhoValido()
    {
        return TamanhoPagina > 50
            ? 50
            : TamanhoPagina;
    }
}