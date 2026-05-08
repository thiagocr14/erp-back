using ErpAcademico.Application.DTOs;
using FluentValidation;

namespace ErpAcademico.Application.Validators;

public class EntradaEstoqueValidator
    : AbstractValidator<EntradaEstoqueDto>
{
    public EntradaEstoqueValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty()
            .WithMessage(
                "Produto é obrigatório.");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0)
            .WithMessage(
                "Quantidade deve ser maior que zero.");

        RuleFor(x => x.Observacao)
            .MaximumLength(200)
            .WithMessage(
                "Observação deve ter no máximo 200 caracteres.");
    }
}