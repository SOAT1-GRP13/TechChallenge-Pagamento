using Domain.Base.Messages;

namespace Application.Pagamentos.MercadoPago.Commands
{
    public class StatusPagamentoFakeCommand : Command<bool>
    {
        public StatusPagamentoFakeCommand(Guid id, string topic)
        {
            Id = id;
            Topic = topic;
        }

        public Guid Id { get; set; }
        public string Topic { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new StatusPagamentoFakeValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}