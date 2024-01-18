using AutoMapper;
using Domain.Pedidos;
using Domain.Base.DomainObjects;
using Domain.MercadoPago;

namespace Application.Pedidos.UseCases
{
    public class PedidoUseCase : IPedidoUseCase
    {
        #region Propriedades
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMercadoPagoRepository _mercadoPagoRepository;
        #endregion

        #region Construtor
        public PedidoUseCase(
            IPedidoRepository pedidoRepository,
            IMercadoPagoRepository mercadoPagoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _mercadoPagoRepository = mercadoPagoRepository;
        }
        #endregion

        #region Use Cases
        public async Task TrocaStatusPedido(Guid idPedido, PedidoStatus novoStatus)
        {
            var Dto = new AtualizaStatus(idPedido, novoStatus);
            await _pedidoRepository.Atualizar(Dto);
        }
        #endregion

    }
}
