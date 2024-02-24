using Domain.MercadoPago;
using Domain.Pedidos;
using Moq;

namespace Application.Tests.Mock.Repositories
{
    public static class MockMercadoPagoRepository
    {
        public static Mock<IMercadoPagoRepository> GetMercadoPagoRepository()
        {
            var mockRepo = new Mock<IMercadoPagoRepository>();

            var mecadoPagoStatus = new MercadoPagoOrderStatus()
            {
                Id = 123,
                Status = "closed",
                External_reference = "external"
            };

            mockRepo.Setup(r => r.GeraPedidoQrCode(It.IsAny<MercadoPagoOrder>())).ReturnsAsync("sucesso");
            mockRepo.Setup(r => r.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(mecadoPagoStatus);

            return mockRepo;
        }
    }
}