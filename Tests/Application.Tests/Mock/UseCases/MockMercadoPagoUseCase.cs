using Application.Pagamentos.MercadoPago.Gateways;
using Application.Pagamentos.MercadoPago.UseCases;
using Application.Tests.Mock.Repositories;
using Moq;

namespace Application.Tests.Mock.UseCases
{
    public static class MockMercadoPagoUseCase
    {
        public static MercadoPagoUseCase GetMercadoPagoUseCase()
        {
            var mercadoPagoRepository = MockMercadoPagoRepository.GetMercadoPagoRepository();
            var pedidoQrRepository = MockPedidosQRRepository.GetPedidosQRRepository();


            return new MercadoPagoUseCase(mercadoPagoRepository.Object, pedidoQrRepository.Object);
        }

        public static Mock<IMercadoPagoUseCase> GetMecadoPagoUseCaseMock()
        {
            var mockUseCase = new Mock<IMercadoPagoUseCase>();

            return mockUseCase;
        }
    }
}