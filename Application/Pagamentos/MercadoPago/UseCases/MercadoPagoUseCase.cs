using Application.Pagamentos.MercadoPago.UseCases;
using Domain.MercadoPago;
using Domain.PedidosQR;
using Domain.PedidosQR.Interface;

namespace Application.Pagamentos.MercadoPago.Gateways
{
    public class MercadoPagoUseCase : IMercadoPagoUseCase
    {
        private readonly IMercadoPagoRepository _mercadoPagoRepository;
                private readonly IPedidosQRRepository _pedidoQrRepository;

        public MercadoPagoUseCase(IMercadoPagoRepository mercadoPagoRepository, IPedidosQRRepository pedidoQrRepository)
        {
            _mercadoPagoRepository = mercadoPagoRepository;
            _pedidoQrRepository = pedidoQrRepository;
        }

        public async Task<string> GerarQRCode(MercadoPagoOrder order)
        {
            return await _mercadoPagoRepository.GeraPedidoQrCode(order);
        }

        public async Task<MercadoPagoOrderStatus> PegaStatusPedido(long id)
        {
            return await _mercadoPagoRepository.PegaStatusPedido(id);
        }

        public async Task SalvaPedidoQR(QrCodeDTO dto)
        {
            await _pedidoQrRepository.SalvaPedidoQR(dto);
        }

        public async Task<QrCodeDTO> BuscaPedidoQr(string pedidoId)
        {
            return await _pedidoQrRepository.BuscaPedidoQr(pedidoId);
        }
    }

}