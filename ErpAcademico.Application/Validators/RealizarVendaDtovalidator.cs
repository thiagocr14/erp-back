using ErpAcademico.Application.DTOs;
using FluentValidation;

namespace ErpAcademico.Application.Validators;

public class RealizarVendaDtoValidator
    : AbstractValidator<RealizarVendaDto>
{
    public RealizarVendaDtoValidator()
    {
        RuleFor(x => x.Itens)
            .NotEmpty()
            .WithMessage(
                "A venda deve possuir itens.");

        RuleForEach(x => x.Itens)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProdutoId)
                    .NotEmpty()
                    .WithMessage(
                        "Produto inválido.");

                item.RuleFor(i => i.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(
                        "Quantidade deve ser maior que zero.");
            });
    }
}