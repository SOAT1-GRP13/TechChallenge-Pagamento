using Domain.MercadoPago;
using Domain.PedidosQR;

namespace Application.Pagamentos.MercadoPago.UseCases
{

    public interface IMercadoPagoUseCase
    {
        Task<MercadoPagoOrderStatus> PegaStatusPedido(long id);
        Task<string> GerarQRCode(MercadoPagoOrder order);
        Task SalvaPedidoQR(QrCodeDTO dto);
        Task<QrCodeDTO> BuscaPedidoQr(string pedidoId);
    }
}
