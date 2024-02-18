namespace Domain.PedidosQR.Interface
{

    public interface IPedidosQRRepository
    {
        Task SalvaPedidoQR(QrCodeDTO dto);
        Task<QrCodeDTO> BuscaPedidoQr(string pedidoId);
    }

}