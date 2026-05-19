namespace ErpAcademico.Domain.Exceptions;

public class NaoEncontradoException
    : Exception
{
    public NaoEncontradoException(
        string mensagem)
        : base(mensagem)
    {
    }
}