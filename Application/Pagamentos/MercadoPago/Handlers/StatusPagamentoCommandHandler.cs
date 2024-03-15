using System.Text.Json;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Configuration;
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
        private readonly Secrets _settings;

        public StatusPagamentoCommandHandler(
         IMediatorHandler mediatorHandler, IMercadoPagoUseCase mercadoPagoUseCase, IRabbitMQService rabbitMQService, IOptions<Secrets> options)
        {
            _mediatorHandler = mediatorHandler;
            _mercadoPagoUseCase = mercadoPagoUseCase;
            _rabbitMQService = rabbitMQService;
            _settings = options.Value;
        }

        public async Task<bool> Handle(StatusPagamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                var pedidoStatus = await _mercadoPagoUseCase.PegaStatusPedido(request.Id);
                var pedidoQR = await _mercadoPagoUseCase.BuscaPedidoQr(pedidoStatus.External_reference);
                var pedido = new PedidoStatus(pedidoStatus.External_reference, pedidoQR.ClienteEmail);
                string mensagem = JsonSerializer.Serialize(pedido);

                if (pedidoStatus.Status == "closed")
                {
                    _rabbitMQService.PublicaMensagem(_settings.ExchangePedidoPago, mensagem);
                }
                else if (pedidoStatus.Status == "expired")
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