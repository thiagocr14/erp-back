using ErpAcademico.Application.DTOs;
using FluentValidation;

namespace ErpAcademico.Application.Validators;

public class AjusteEstoqueDtoValidator
    : AbstractValidator<AjusteEstoqueDto>
{
    public AjusteEstoqueDtoValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty()
            .WithMessage(
                "Produto é obrigatório.");

        RuleFor(x => x.NovaQuantidade)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Nova quantidade inválida.");
    }
}
