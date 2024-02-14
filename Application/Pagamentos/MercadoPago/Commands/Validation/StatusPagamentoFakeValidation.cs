using FluentValidation;

namespace Application.Pagamentos.MercadoPago.Commands;

public class StatusPagamentoFakeValidation : AbstractValidator<StatusPagamentoFakeCommand>
{
    public StatusPagamentoFakeValidation()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Action é obrigatório");

        RuleFor(x => x.Topic)
        .NotEmpty()
        .WithMessage("Topic é obrigatório");
    }

}
