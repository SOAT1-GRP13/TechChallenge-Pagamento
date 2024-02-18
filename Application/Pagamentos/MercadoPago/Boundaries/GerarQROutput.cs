namespace Application.Pagamentos.MercadoPago.Boundaries
{
    public class GerarQROutput
    {
        public GerarQROutput()
        {
            Qr_data = string.Empty;
            PedidoId = Guid.Empty;

        }

        public GerarQROutput(string qr, string pedidoId)
        {
            Qr_data = qr;
            PedidoId = Guid.Parse(pedidoId);
        }

        public string Qr_data { get; set; }
        public Guid PedidoId { get; set; }
    }

}