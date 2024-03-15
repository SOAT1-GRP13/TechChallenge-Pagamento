using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.MercadoPago;
using Domain.PedidosQR;
using MediatR;

namespace Application.Pagamentos.MercadoPago.Handlers
{
    public class GerarQRCommandHandler :
        IRequestHandler<GerarQRCommand, bool>
    {
        private readonly IMercadoPagoUseCase _mercadoPagoUseCase;
        private readonly IMediatorHandler _mediatorHandler;

        public GerarQRCommandHandler(
         IMediatorHandler mediatorHandler, IMercadoPagoUseCase mercadoPagoUseCase)
        {
            _mediatorHandler = mediatorHandler;
            _mercadoPagoUseCase = mercadoPagoUseCase;
        }

        public async Task<bool> Handle(GerarQRCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                var dto = new MercadoPagoOrder(request.Input);
                var response = await _mercadoPagoUseCase.GerarQRCode(dto);

                var pedidoQr = new QrCodeDTO(response, request.Input.PedidoId.ToString(), request.Input.ClienteEmail);
                await _mercadoPagoUseCase.SalvaPedidoQR(pedidoQr);
            }
            else
            {
                foreach (var error in request.ValidationResult.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                }
            }
            return true;
        }
    }
}