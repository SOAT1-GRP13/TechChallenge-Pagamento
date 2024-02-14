namespace Domain.PedidosQR.Interface
{

    public interface IPedidosQRRepository
    {
        Task SalvaPedidoQR(QrCodeDTO dto);
    }

}