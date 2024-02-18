using Application.Pagamentos.MercadoPago.Boundaries;
using Domain.Base.Messages;

namespace Application.Pagamentos.MercadoPago.Commands
{
    public class BuscarQRCommand : Command<GerarQROutput>
    {
        public BuscarQRCommand(Guid input)
        {
            Input = input;
        }

        public Guid Input {get;set;}

        public override bool EhValido()
        {
            ValidationResult = new BuscarQRValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}