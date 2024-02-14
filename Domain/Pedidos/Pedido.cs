namespace Domain.Pedidos
{
    public class Pedido
    {
        public Pedido(Guid pedidoId, Guid clienteId, decimal subTotal,
         decimal valorTotal, List<PedidoItem> items)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            SubTotal = subTotal;
            ValorTotal = valorTotal;
            Items = items;
        }

        public Pedido(){
            
        }

        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValorTotal { get; set; }

        public List<PedidoItem> Items { get; set; } = new List<PedidoItem>();
    }
}
