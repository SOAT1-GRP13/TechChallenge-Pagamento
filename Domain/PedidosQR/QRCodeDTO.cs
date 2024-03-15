
namespace Domain.PedidosQR
{
    public class QrCodeDTO
    {
        public QrCodeDTO()
        {
            QRData = string.Empty;
            PedidoId = string.Empty;
            ClienteEmail = string.Empty;
        }

        public QrCodeDTO(string qrData, string pedidoId, string clienteEmail)
        {
            QRData = qrData;
            PedidoId = pedidoId;
            ClienteEmail = clienteEmail;
        }

        public string QRData { get; set; }
        public string PedidoId { get; set; }
        public string ClienteEmail {get;set;}
    }
}