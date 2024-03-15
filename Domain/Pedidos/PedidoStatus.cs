namespace Domain.Pedidos
{
    public class PedidoStatus
    {
        public PedidoStatus(string pedidoId, string clienteEmail)
        {
            PedidoId = Guid.Parse(pedidoId);
            ClienteEmail = clienteEmail;
        }
        public Guid PedidoId { get; set; }
        public string ClienteEmail { get; set; }
    }
}