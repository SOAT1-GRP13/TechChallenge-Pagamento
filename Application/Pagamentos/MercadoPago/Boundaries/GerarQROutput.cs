namespace Application.Pagamentos.MercadoPago.Boundaries
{
    public class GerarQROutput
    {
        public GerarQROutput()
        {
            Qr_data = string.Empty;

        }

        public GerarQROutput(string qr)
        {
            Qr_data = qr;

        }

        public string Qr_data { get; set; }
    }

}