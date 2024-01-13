using Application.Pagamentos.MercadoPago.Boundaries;
using FluentValidation;

namespace Application.Pagamentos.MercadoPago.Commands;

public class GerarQRValidation : AbstractValidator<OrderInput>
{
    public GerarQRValidation()
    {
        RuleFor(x => x.Title)
        .NotEmpty()
        .WithMessage("Titulo é obrigatório");

        RuleFor(x => x.External_reference)
        .NotEmpty()
        .WithMessage("Id do pedido é obrigatório");

        RuleFor(x => x.Expiration_date)
        .NotEmpty()
        .WithMessage("Expiration Date é obrigatório");

        RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description é obrigatório");

        RuleFor(x => x.Total_amount)
        .NotEmpty()
        .NotEqual(0)
        .WithMessage("Total amount é obrigatório");

        RuleFor(x => x.Items)
        .NotEmpty()
        .WithMessage("Ao menos 1 item é necessario");
    }

}
