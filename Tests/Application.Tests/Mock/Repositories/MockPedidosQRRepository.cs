using Domain.PedidosQR;
using Domain.PedidosQR.Interface;
using Moq;

namespace Application.Tests.Mock.Repositories
{
    public static class MockPedidosQRRepository
    {
        public static Mock<IPedidosQRRepository> GetPedidosQRRepository()
        {
            var mockRepo = new Mock<IPedidosQRRepository>();

            var qrCodeDTO = new QrCodeDTO("sucesso", "sucesso", string.Empty);

            mockRepo.Setup(r => r.BuscaPedidoQr(It.IsAny<string>())).ReturnsAsync(qrCodeDTO);

            return mockRepo;
        }
    }
}