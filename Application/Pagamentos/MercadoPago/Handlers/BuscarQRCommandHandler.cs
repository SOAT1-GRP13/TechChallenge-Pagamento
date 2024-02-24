using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;

namespace Application.Pagamentos.MercadoPago.Handlers
{
    public class BuscarQRCommandHandler :
        IRequestHandler<BuscarQRCommand, GerarQROutput>
    {
        private readonly IMercadoPagoUseCase _mercadoPagoUseCase;
        private readonly IMediatorHandler _mediatorHandler;

        public BuscarQRCommandHandler(
         IMediatorHandler mediatorHandler, IMercadoPagoUseCase mercadoPagoUseCase)
        {
            _mediatorHandler = mediatorHandler;
            _mercadoPagoUseCase = mercadoPagoUseCase;
        }

        public async Task<GerarQROutput> Handle(BuscarQRCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                var pedidoQr = await _mercadoPagoUseCase.BuscaPedidoQr(request.Input.ToString());

                return new GerarQROutput(pedidoQr.QRData, pedidoQr.PedidoId);
            }
            else
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                }
            }
            return new GerarQROutput();
        }
    }
}