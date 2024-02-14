using System.Text.Json;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.DomainObjects;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Pedidos;
using Domain.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Pagamentos.MercadoPago.Handlers
{
    public class StatusPagamentoCommandHandler :
        IRequestHandler<StatusPagamentoCommand, bool>
    {

        private readonly IMercadoPagoUseCase _mercadoPagoUseCase;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly RabbitMQOptions _options;

        public StatusPagamentoCommandHandler(
         IMediatorHandler mediatorHandler, IMercadoPagoUseCase mercadoPagoUseCase, IRabbitMQService rabbitMQService, RabbitMQOptions options)
        {
            _mediatorHandler = mediatorHandler;
            _mercadoPagoUseCase = mercadoPagoUseCase;
            _rabbitMQService = rabbitMQService;
            _options = options;
        }

        public async Task<bool> Handle(StatusPagamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                try
                {
                    //TODO publicar na fila
                    var pedidoStatus = await _mercadoPagoUseCase.PegaStatusPedido(request.Id);

                    if (pedidoStatus.Status == "closed")
                    {
                        var pedido = new PedidoPago(pedidoStatus.External_reference);

                        string mensagem = JsonSerializer.Serialize(pedido);
                        _rabbitMQService.PublicaMensagem(_options.QueuePedidoPago, mensagem);
                    }

                    return true;
                }
                catch (DomainException ex)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
                }
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