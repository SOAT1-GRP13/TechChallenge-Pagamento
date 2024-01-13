using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.UseCases;
using Application.Pedidos.UseCases;
using AutoMapper;
using Domain.Base.Communication.Mediator;
using Domain.Base.DomainObjects;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.MercadoPago;
using Domain.Pedidos;
using MediatR;

namespace Application.Pagamentos.MercadoPago.Handlers
{
    public class GerarQRCommandHandler :
        IRequestHandler<GerarQRCommand, GerarQROutput>
    {

        private readonly IMapper _mapper;
        private readonly IMercadoPagoUseCase _mercadoPagoUseCase;
        private readonly IMediatorHandler _mediatorHandler;

        public GerarQRCommandHandler(
         IMediatorHandler mediatorHandler, IMercadoPagoUseCase mercadoPagoUseCase, IMapper mapper)
        {
            _mediatorHandler = mediatorHandler;
            _mercadoPagoUseCase = mercadoPagoUseCase;
            _mapper = mapper;
        }

        public async Task<GerarQROutput> Handle(GerarQRCommand request, CancellationToken cancellationToken)
        {
            if (request.EhValido())
            {
                try
                {
                    var dto = _mapper.Map<MercadoPagoOrder>(request.Input);
                    var response =  await _mercadoPagoUseCase.GerarQRCode(dto);

                    return new GerarQROutput(response);
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
            return new GerarQROutput();
        }
    }
}