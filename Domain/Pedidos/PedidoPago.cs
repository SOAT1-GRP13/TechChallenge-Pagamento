namespace Domain.Pedidos
{
    public class PedidoPago
    {
        public PedidoPago(string pedidoId)
        {
            PedidoId = Guid.Parse(pedidoId);
        }
        public Guid PedidoId {get;set;}
    }
}