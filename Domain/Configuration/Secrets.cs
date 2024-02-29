namespace Domain.Configuration
{
    public class Secrets
    {
        public Secrets()
        {
            MercadoPagoUserId = string.Empty;
            AccesToken = string.Empty;
            Notification_url = string.Empty;
            External_Pos_Id = string.Empty;
            Rabbit_Hostname = string.Empty;
            Rabbit_Password = string.Empty;
            ExchangePedidoConfirmado = string.Empty;
            Rabbit_Username = string.Empty;
            ExchangePedidoConfirmado = string.Empty;
            ExchangePedidoRecusado = string.Empty;
            ExchangePedidoPago = string.Empty;
            QueuePedidoConfirmado = string.Empty;
            Rabbit_VirtualHost = string.Empty;
        }

        public string MercadoPagoUserId { get; set; }
        public string AccesToken { get; set; }
        public string Notification_url { get; set; }
        public string External_Pos_Id { get; set; }
        public string Rabbit_Hostname { get; set; }
        public int Rabbit_Port { get; set; }
        public string Rabbit_Username { get; set; }
        public string Rabbit_Password { get; set; }
        public string ExchangePedidoConfirmado { get; set; }
        public string ExchangePedidoRecusado { get; set; }
        public string ExchangePedidoPago { get; set; }
        public string QueuePedidoConfirmado { get; set; }
        public string Rabbit_VirtualHost {get;set;}
    }
}