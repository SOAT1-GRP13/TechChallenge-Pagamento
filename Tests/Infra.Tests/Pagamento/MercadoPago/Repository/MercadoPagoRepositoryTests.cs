using Domain.MercadoPago;
using Domain.Pedidos;
using Infra.Tests.Mock.Repositories;

namespace Infra.Tests.Pagamento.MercadoPago.Repository
{
    public class MercadoPagoRepositoryTests
    {
        [Fact]
        public async Task AoGeraPedidoQrCode_EOcorrerErro_DeveGerarStringVazia()
        {
            //Arrange
            var pedido = new Pedido();

            var order = new MercadoPagoOrder(pedido)
            {
                Notification_url = "teste"
            };

            var repository = MockMercadoPagoRepository.GetMercadoPagoRepository().Object;

            // Act
            var result = await repository.GeraPedidoQrCode(order);

            //Assert
            Assert.Equal("", result);
        }

        [Fact]
        public async Task AoPegaStatusPedido_EOcorrerErro_DeveGerarStringVazia()
        {
            var repository = MockMercadoPagoRepository.GetMercadoPagoRepository().Object;

            // Act
            var result = await repository.PegaStatusPedido(12345);

            //Assert
            var badRequestResult = Assert.IsType<MercadoPagoOrderStatus>(result);
            Assert.Equal(0, badRequestResult.Id);
        }
    }
}
