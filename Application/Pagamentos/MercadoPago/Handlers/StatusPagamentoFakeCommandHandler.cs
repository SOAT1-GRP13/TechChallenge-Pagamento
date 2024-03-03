using System.Text.Json;
using Application.Pagamentos.MercadoPago.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Configuration;
using Domain.Pedidos;
using Domain.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Pagamentos.MercadoPago.Handlers
{
    public class StatusPagamentoFakeCommandHandler :
        IRequestHandler<StatusPagamentoFakeCommand, bool>
    {

        private readonly IRabbitMQService _rabbitMQService;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly Secrets _settings;


        public StatusPagamentoFakeCommandHandler(
         IMediatorHandler mediatorHandler, IRabbitMQService rabbitMQService, IOptions<Secrets> options)
        {
            _mediatorHandler = mediatorHandler;
            _rabbitMQService = rabbitMQService;
            _settings = options.Value;
        }

        public async Task<bool> Handle(StatusPagamentoFakeCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                var pedidoId = request.Id.ToString();


                var pedido = new PedidoStatus(pedidoId);

                string mensagem = JsonSerializer.Serialize(pedido);

                if (request.Status == "closed")
                {
                    _rabbitMQService.PublicaMensagem(_settings.ExchangePedidoPago, mensagem);
                }
                else
                {
                    _rabbitMQService.PublicaMensagem(_settings.ExchangePedidoRecusado, mensagem);
                }

                return true;
            }
            else
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                }
            }
            return false;
        }
    }
}