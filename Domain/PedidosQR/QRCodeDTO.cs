
namespace Domain.PedidosQR
{
    public class QrCodeDTO
    {
        public QrCodeDTO()
        {
            QRData = string.Empty;
            PedidoId = string.Empty;
        }

        public QrCodeDTO(string qrData, string pedidoId)
        {
            QRData = qrData;
            PedidoId = pedidoId;
        }

        public string QRData { get; set; }
        public string PedidoId { get; set; }
    }
}