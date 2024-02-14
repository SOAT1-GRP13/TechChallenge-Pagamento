using Domain.MercadoPago;
using Moq;

namespace Infra.Tests.Mock.Repositories
{
    public static class MockMercadoPagoRepository
    {
        public static Mock<IMercadoPagoRepository> GetMercadoPagoRepository()
        {
            var mockRepo = new Mock<IMercadoPagoRepository>();

            mockRepo.Setup(r => r.GeraPedidoQrCode(It.IsAny<MercadoPagoOrder>())).ReturnsAsync(string.Empty);
            mockRepo.Setup(r => r.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(new MercadoPagoOrderStatus());

            return mockRepo;
        }
    }
}