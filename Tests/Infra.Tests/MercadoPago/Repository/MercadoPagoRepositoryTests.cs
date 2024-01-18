using Domain.Configuration;
using Domain.MercadoPago;
using Domain.Pedidos;
using Infra.MercadoPago.Repository;
using Microsoft.Extensions.Options;
using Moq;

namespace Infra.Tests.MercadoPago.Repository
{
    public class MercadoPagoRepositoryTests
    {
        [Fact]
        public async Task AoGeraPedidoQrCode_EOcorrerErro_DeveGerarStringVazia()
        {
            //Arrange
            var mockOptions = new Mock<IOptions<Secrets>>();
            var secrets = new Secrets
            {
                // Configure os valores necessários
                MercadoPagoUserId = "12345",
                External_Pos_Id = "123",
                AccesToken = "access_token",
                Notification_url = "http://test-notification-url.com"
            };
            mockOptions.Setup(opt => opt.Value).Returns(secrets);

            var pedido = new Pedido(Guid.NewGuid(), false, 0, 10);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto 1", 1, 10);
            var itens = new List<OrderItem>
            {
                new OrderItem(pedidoItem)
            };
            var order = new MercadoPagoOrder(pedido, itens, "Teste");

            var repository = new MercadoPagoRepository(mockOptions.Object);

            // Act
            var result = await repository.GeraPedidoQrCode(order);

            //Assert
            Assert.Equal("", result);
        }

        [Fact]
        public async Task AoPegaStatusPedido_EOcorrerErro_DeveGerarStringVazia()
        {
            //Arrange
            var mockOptions = new Mock<IOptions<Secrets>>();
            var secrets = new Secrets
            {
                // Configure os valores necessários
                MercadoPagoUserId = "12345",
                External_Pos_Id = "123",
                AccesToken = "access_token",
                Notification_url = "http://test-notification-url.com"
            };
            mockOptions.Setup(opt => opt.Value).Returns(secrets);

            var repository = new MercadoPagoRepository(mockOptions.Object);

            // Act
            var result = await repository.PegaStatusPedido(12345);

            //Assert
            var badRequestResult = Assert.IsType<MercadoPagoOrderStatus>(result);
            Assert.Equal(0, badRequestResult.Id);
        }
    }
}
