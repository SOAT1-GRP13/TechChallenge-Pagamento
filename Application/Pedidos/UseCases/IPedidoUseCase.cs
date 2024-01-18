using Domain.Pedidos;

namespace Application.Pedidos.UseCases
{
    public interface IPedidoUseCase
    {
        Task TrocaStatusPedido(Guid idPedido, PedidoStatus novoStatus);
    }
}
