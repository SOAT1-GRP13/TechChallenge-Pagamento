using Domain.PedidosQR;
using Domain.PedidosQR.Interface;
using Moq;

namespace Infra.Tests.Mock.Repositories
{
    public static class MockPedidosQRRepository
    {
        public static Mock<IPedidosQRRepository> GetPedidosQRRepository()
        {
            var mockRepo = new Mock<IPedidosQRRepository>();

            mockRepo.Setup(r => r.SalvaPedidoQR(new QrCodeDTO("sucesso", "sucesso", string.Empty)));
            mockRepo.Setup(r => r.SalvaPedidoQR(new QrCodeDTO("erro", "erro", string.Empty))).ThrowsAsync(new Exception("Simulando uma exceção"));
            mockRepo.Setup(r => r.BuscaPedidoQr("erro")).ReturnsAsync(new QrCodeDTO());
            mockRepo.Setup(r => r.BuscaPedidoQr("sucesso")).ReturnsAsync(new QrCodeDTO("sucesso", "sucesso", string.Empty));

            return mockRepo;
        }
    }
}