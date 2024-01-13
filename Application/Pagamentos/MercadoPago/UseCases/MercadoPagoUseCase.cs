using Application.Pagamentos.MercadoPago.UseCases;
using Domain.MercadoPago;

namespace Application.Pagamentos.MercadoPago.Gateways
{
    public class MercadoPagoUseCase : IMercadoPagoUseCase
    {
        private readonly IMercadoPagoRepository _mercadoPagoRepository;

        public MercadoPagoUseCase(IMercadoPagoRepository mercadoPagoRepository)
        {
            _mercadoPagoRepository = mercadoPagoRepository;
        }

        public async Task<string> GerarQRCode(MercadoPagoOrder order)
        {
            return await _mercadoPagoRepository.GeraPedidoQrCode(order);
        }

        public async Task<MercadoPagoOrderStatus> PegaStatusPedido(long id)
        {
            return await _mercadoPagoRepository.PegaStatusPedido(id);
        }
    }

}