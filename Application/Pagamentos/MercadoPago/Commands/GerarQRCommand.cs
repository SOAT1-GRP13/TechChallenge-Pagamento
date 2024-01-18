using Application.Pagamentos.MercadoPago.Boundaries;
using Domain.Base.Messages;

namespace Application.Pagamentos.MercadoPago.Commands
{
    public class GerarQRCommand : Command<GerarQROutput>
    {
        public GerarQRCommand(OrderInput input)
        {
            Input = input;
        }

        public OrderInput Input {get;set;}

        public override bool EhValido()
        {
            ValidationResult = new GerarQRValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}