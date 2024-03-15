using Application.Pagamentos.MercadoPago.Gateways;
using Application.Pagamentos.MercadoPago.UseCases;
using Application.Tests.Mock.UseCases;
using Domain.MercadoPago;
using Domain.Pedidos;
using Domain.PedidosQR;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.UseCases
{
    public class MercadoPagoUseCaseTests
    {
        private readonly Mock<IMercadoPagoUseCase> _useCaseMock;
        private readonly MercadoPagoUseCase _usecase;

        public MercadoPagoUseCaseTests()
        {
            _usecase = MockMercadoPagoUseCase.GetMercadoPagoUseCase();
            _useCaseMock = MockMercadoPagoUseCase.GetMecadoPagoUseCaseMock();
        }

        [Fact]
        public async void DeveRetornarString_AoChamarGerarQrCode()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "teste", 1, 10, 10);
            var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid(), 0, 10,
             new List<PedidoItem>() { pedidoItem }, string.Empty);
            var mercadoPagoOrder = new MercadoPagoOrder(pedido);

            _useCaseMock.Setup(u => u.GerarQRCode(It.IsAny<MercadoPagoOrder>())).ReturnsAsync("sucesso");

            var result = await _usecase.GerarQRCode(mercadoPagoOrder);

            var stringRetornado = Assert.IsType<string>(result);
            Assert.Equal("sucesso", stringRetornado);

        }

        [Fact]
        public async void DeveRetornarStatus_AoChamarPegaStatusPedido()
        {
            // Arrange
            var mecadoPagoStatus = new MercadoPagoOrderStatus()
            {
                Id = 123,
                Status = "closed",
                External_reference = "external"
            };

            _useCaseMock.Setup(u => u.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(mecadoPagoStatus);

            var result = await _usecase.PegaStatusPedido(123);

            var statusRetornado = Assert.IsType<MercadoPagoOrderStatus>(result);
            Assert.Equal("closed", statusRetornado.Status);
            Assert.Equal("external", statusRetornado.External_reference);
            Assert.Equal(123, statusRetornado.Id);

        }

        [Fact]
        public async void DeveRetornarPedidoQR_AoChamarBuscaPedidoQr()
        {
            // Arrange
            var qrCodeDTO = new QrCodeDTO("sucesso", "sucesso", string.Empty);

            _useCaseMock.Setup(u => u.BuscaPedidoQr(It.IsAny<string>())).ReturnsAsync(qrCodeDTO);

            var result = await _usecase.BuscaPedidoQr("123");

            var qrRetornado = Assert.IsType<QrCodeDTO>(result);
            Assert.Equal("sucesso", qrRetornado.QRData);
            Assert.Equal("sucesso", qrRetornado.PedidoId);

        }

    }
}