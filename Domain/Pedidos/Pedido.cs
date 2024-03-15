namespace Domain.Pedidos
{
    public class Pedido
    {
        public Pedido(Guid pedidoId, Guid clienteId, decimal subTotal,
         decimal valorTotal, List<PedidoItem> items, string clienteEmail)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            SubTotal = subTotal;
            ValorTotal = valorTotal;
            Items = items;
            ClienteEmail = clienteEmail;
        }

        public Pedido()
        {
            ClienteEmail = string.Empty;
        }

        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public string ClienteEmail { get; set; }

        public List<PedidoItem> Items { get; set; } = new List<PedidoItem>();
    }
}
