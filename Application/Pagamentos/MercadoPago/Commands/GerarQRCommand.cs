using Application.Pagamentos.MercadoPago.Boundaries;
using Domain.Base.Messages;
using Domain.Pedidos;

namespace Application.Pagamentos.MercadoPago.Commands
{
    public class GerarQRCommand : Command<bool>
    {
        public GerarQRCommand(Pedido input)
        {
            Input = input;
        }

        public Pedido Input {get;set;}

        public override bool EhValido()
        {
            ValidationResult = new GerarQRValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}