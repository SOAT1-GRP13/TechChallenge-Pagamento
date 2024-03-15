using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.MercadoPago;
using Domain.Pedidos;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class GerarQRCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoValidacaoFalha()
        {
            // Arrange
            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(new Pedido()); // Dados inválidos para falhar na validação
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(7));
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucesso()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "teste", 1, 10, 10);
            var pedido = new Pedido(Guid.NewGuid(), Guid.NewGuid(), 0, 10,
             new List<PedidoItem>() { pedidoItem }, string.Empty);

            var mercadoPagoOrder = new MercadoPagoOrder(pedido);

            

            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            mercadoPagoUseCaseMock.Setup(m => m.GerarQRCode(It.IsAny<MercadoPagoOrder>())).ReturnsAsync("qr_data_teste");

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(pedido);
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuantoTiverErros()
        {
            // Arrange
            var pedido = new Pedido();

            var mercadoPagoOrder = new MercadoPagoOrder {};

            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(pedido);
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<bool>(result);
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(7));
            Assert.True(result);
        }
    }
}
