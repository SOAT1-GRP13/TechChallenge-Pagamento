namespace Domain.Pedidos
{
    public class PedidoStatus
    {
        public PedidoStatus(string pedidoId)
        {
            PedidoId = Guid.Parse(pedidoId);
        }
        public Guid PedidoId {get;set;}
    }
}