using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Configuration;
using Domain.MercadoPago;
using Domain.RabbitMQ;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class StatusPagamentoCommandHandlerTests
    {
        private readonly Mock<IMercadoPagoUseCase> _mercadoPagoUseCaseMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly Mock<IOptions<Secrets>> _mockOptions;
        private readonly Secrets _secrets;
        private readonly StatusPagamentoCommandHandler _handler;
        public StatusPagamentoCommandHandlerTests()
        {
            _mockOptions = new Mock<IOptions<Secrets>>();
            _secrets = new Secrets () 
            {
                ExchangePedidoPago = "exc_pedido_pago",
                ExchangePedidoRecusado = "exc_pedido_recusado"

            };
            _mockOptions.Setup(opt => opt.Value).Returns(_secrets);
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            _handler = new StatusPagamentoCommandHandler(
                _mediatorHandlerMock.Object,
                _mercadoPagoUseCaseMock.Object,
            _rabbitMQServiceMock.Object,
            _mockOptions.Object
            );
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoValidacaoFalha()
        {
            // Arrange
            var command = new StatusPagamentoCommand(0, ""); // Dados inválidos para falhar na validação

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucessoEAprovado()
        {
            // Arrange
            var command = new StatusPagamentoCommand(123, "payment");
            var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "closed", External_reference = Guid.NewGuid().ToString() };
            _mercadoPagoUseCaseMock.Setup(m => m.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(mercadoPagoOrderStatus);

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
            var command = new StatusPagamentoCommand(123, "payment");
            var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "expired", External_reference = Guid.NewGuid().ToString() };
            _mercadoPagoUseCaseMock.Setup(m => m.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(mercadoPagoOrderStatus);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _rabbitMQServiceMock.Verify(r => r.PublicaMensagem(_secrets.ExchangePedidoRecusado, It.IsAny<string>()), Times.Once());
        }

    }
}
