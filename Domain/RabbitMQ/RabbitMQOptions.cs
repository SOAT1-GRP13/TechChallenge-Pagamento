namespace Domain.RabbitMQ
{
    public class RabbitMQOptions
    {
        public RabbitMQOptions() { 
            Hostname = string.Empty;
            Port = 5672;
            Username = string.Empty;
            Password = string.Empty;
            ExchangePedidoConfirmado = string.Empty;
            ExchangePedidoRecusado = string.Empty;
            ExchangePedidoPago = string.Empty;
            QueuePedidoConfirmado = string.Empty;
            VirtualHost = string.Empty;
        }

        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangePedidoConfirmado { get; set; }
        public string ExchangePedidoRecusado { get; set; }
        public string ExchangePedidoPago { get; set; }
        public string QueuePedidoConfirmado { get; set; }
        public string VirtualHost {get;set;}
    }
}
