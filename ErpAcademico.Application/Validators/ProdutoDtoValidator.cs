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
            .WithMessage(
                "Nome é obrigatório.");

        RuleFor(x => x.PrecoVenda)
            .GreaterThan(0)
            .WithMessage(
                "Preço de venda deve ser maior que zero.");

        RuleFor(x => x.PrecoCusto)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Preço de custo inválido.");

        RuleFor(x => x.QuantidadeAtual)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Quantidade inválida.");

        RuleFor(x => x.EstoqueMinimo)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Estoque mínimo inválido.");

        RuleFor(x => x.EstoqueIdeal)
            .GreaterThanOrEqualTo(
                x => x.EstoqueMinimo)
            .WithMessage(
                "Estoque ideal não pode ser menor que o mínimo.");
    }
}