using ErpAcademico.Application.DTOs;
using FluentValidation;

namespace ErpAcademico.Application.Validators;

public class ProdutoDtoValidator
    : AbstractValidator<ProdutoDto>
{
    public ProdutoDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.");

        RuleFor(x => x.PrecoVenda)
            .GreaterThan(0)
            .WithMessage(
                "O preço de venda deve ser maior que zero.");

        RuleFor(x => x.PrecoCusto)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "O preço de custo não pode ser negativo.");

        RuleFor(x => x.QuantidadeAtual)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "A quantidade atual não pode ser negativa.");

        RuleFor(x => x.EstoqueMinimo)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.EstoqueIdeal)
            .GreaterThanOrEqualTo(x => x.EstoqueMinimo)
            .WithMessage(
                "O estoque ideal deve ser maior ou igual ao mínimo.");
    }
}