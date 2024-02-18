using Application.Pagamentos.MercadoPago.Boundaries;
using Domain.Pedidos;
using FluentValidation;

namespace Application.Pagamentos.MercadoPago.Commands;

public class GerarQRValidation : AbstractValidator<Pedido>
{
    public GerarQRValidation()
    {
        RuleFor(x => x.PedidoId)
        .NotEmpty()
        .WithMessage("PedidoId é obrigatório");

        RuleFor(x => x.SubTotal)
        .NotEmpty()
        .NotEqual(0)
        .WithMessage("SubTotal é obrigatório");

        RuleFor(x => x.ValorTotal)
        .NotEmpty()
        .NotEqual(0)
        .WithMessage("ValorTotal é obrigatório");

        RuleFor(x => x.ClienteId)
         .NotEmpty()
         .WithMessage("ClienteId é obrigatório");


        RuleFor(x => x.Items)
        .NotEmpty()
        .WithMessage("Ao menos 1 item é necessario");
    }

}
