namespace Domain.Pedidos
{
    public interface IPedidoRepository
    {
        Task Atualizar(AtualizaStatus pedido);

    }
}
