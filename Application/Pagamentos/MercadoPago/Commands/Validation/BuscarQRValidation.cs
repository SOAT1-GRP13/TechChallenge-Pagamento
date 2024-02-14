using Application.Pagamentos.MercadoPago.Boundaries;
using Domain.Pedidos;
using FluentValidation;

namespace Application.Pagamentos.MercadoPago.Commands;

public class BuscarQRValidation : AbstractValidator<Guid>
{
    public BuscarQRValidation()
    {
        RuleFor(x => x)
        .NotEmpty()
        .WithMessage("PedidoId é obrigatório");

    }

}
