using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.PedidosQR;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class BuscarQRCommandHandlerTests
    {
        private readonly Mock<IMercadoPagoUseCase> _mercadoPagoUseCaseMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly BuscarQRCommandHandler _handler;
        public BuscarQRCommandHandlerTests()
        {
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            _handler = new BuscarQRCommandHandler(_mediatorHandlerMock.Object, _mercadoPagoUseCaseMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarQROutput_QuandoExecutadoComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var output = new QrCodeDTO("sucesso", pedidoId.ToString());
            var command = new BuscarQRCommand(pedidoId);

            _mercadoPagoUseCaseMock.Setup(x => x.BuscaPedidoQr(pedidoId.ToString())).ReturnsAsync(output);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<GerarQROutput>(result);
            Assert.Equal("sucesso", objectResult.Qr_data);
            Assert.Equal(pedidoId, objectResult.PedidoId);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoNaoEnviadoPedidoId()
        {
            // Arrange
            var pedidoId = Guid.Empty;
            var command = new BuscarQRCommand(pedidoId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        }
    }
}
