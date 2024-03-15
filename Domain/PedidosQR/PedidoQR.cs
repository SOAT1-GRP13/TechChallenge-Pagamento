using Amazon.DynamoDBv2.DataModel;

namespace Domain.PedidosQR
{
    [DynamoDBTable("PedidosQR")]
    public class PedidoQR
    {
        public PedidoQR(string pedidoId, string qrData, string clienteEmail)
        {
            PedidoId = pedidoId;
            QrData = qrData;
            ClienteEmail = clienteEmail;
            Ttl = DateTimeOffset.Now.AddMinutes(20).ToUnixTimeSeconds(); //Após isso ja expirou no mercado pago
        }

        public PedidoQR()
        {
            PedidoId = string.Empty;
            QrData = string.Empty;
            ClienteEmail = string.Empty;
            Ttl = 0;
        }

        [DynamoDBHashKey("pedidoId")]
        public string PedidoId { get; set; }
        
        [DynamoDBProperty("clienteEmail")]
        public string ClienteEmail { get; set; }

        [DynamoDBProperty("qrData")]
        public string QrData { get; set; }

        [DynamoDBProperty("ttl")]
        public long Ttl { get; set; }
    }
}