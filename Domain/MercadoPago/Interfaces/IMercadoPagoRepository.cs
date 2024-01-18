namespace Domain.MercadoPago
{
    public interface IMercadoPagoRepository
    {
        Task<string> GeraPedidoQrCode(MercadoPagoOrder order);

        Task<MercadoPagoOrderStatus> PegaStatusPedido(long id);
    }
}
