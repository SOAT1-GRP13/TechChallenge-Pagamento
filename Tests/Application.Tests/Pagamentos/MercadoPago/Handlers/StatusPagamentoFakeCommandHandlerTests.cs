using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Configuration;
using Domain.MercadoPago;
using Domain.PedidosQR;
using Domain.RabbitMQ;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class StatusPagamentoFakeCommandHandlerTests
    {
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly Mock<IOptions<Secrets>> _mockOptions;
        private readonly Mock<IMercadoPagoUseCase> _mockMercadoPagoUseCase;
        private readonly Secrets _secrets;
        private readonly StatusPagamentoFakeCommandHandler _handler;
        public StatusPagamentoFakeCommandHandlerTests()
        {
            _mockOptions = new Mock<IOptions<Secrets>>();
            _secrets = new Secrets()
            {
                ExchangePedidoPago = "exc_pedido_pago",
                ExchangePedidoRecusado = "exc_pedido_recusado"

            };
            _mockOptions.Setup(opt => opt.Value).Returns(_secrets);
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _mockMercadoPagoUseCase = new Mock<IMercadoPagoUseCase>();
            _handler = new StatusPagamentoFakeCommandHandler(
                _mediatorHandlerMock.Object,
            _rabbitMQServiceMock.Object,
             _mockOptions.Object,
             _mockMercadoPagoUseCase.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalseETerMensagens_QuandoValidacaoFalha()
        {
            // Arrange
            var command = new StatusPagamentoFakeCommand(Guid.Empty, "", ""); // Dados inválidos para falhar na validação

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucessoEPago()
        {
            // Arrange
            var command = new StatusPagamentoFakeCommand(Guid.NewGuid(), "payment", "closed");
            var qrCodeDTO = new QrCodeDTO("sucesso", "sucesso", string.Empty);
            var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "closed", External_reference = Guid.NewGuid().ToString() };
            _mockMercadoPagoUseCase.Setup(x => x.BuscaPedidoQr(It.IsAny<string>())).ReturnsAsync(qrCodeDTO);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _rabbitMQServiceMock.Verify(r => r.PublicaMensagem(_secrets.ExchangePedidoPago, It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucessoERecusado()
        {
            // Arrange
            var command = new StatusPagamentoFakeCommand(Guid.NewGuid(), "payment", "expired");
            var qrCodeDTO = new QrCodeDTO("sucesso", "sucesso", string.Empty);
            var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "closed", External_reference = Guid.NewGuid().ToString() };
            _mockMercadoPagoUseCase.Setup(x => x.BuscaPedidoQr(It.IsAny<string>())).ReturnsAsync(qrCodeDTO);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _rabbitMQServiceMock.Verify(r => r.PublicaMensagem(_secrets.ExchangePedidoRecusado, It.IsAny<string>()), Times.Once());
        }
    }
}
