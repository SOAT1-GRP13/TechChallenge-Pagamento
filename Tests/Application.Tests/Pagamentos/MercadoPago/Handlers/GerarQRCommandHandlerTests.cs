using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using AutoMapper;
using Domain.Base.Communication.Mediator;
using Domain.Base.DomainObjects;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.MercadoPago;
using Domain.Pedidos;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class GerarQRCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarQrVazio_QuandoValidacaoFalha()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(new OrderInput()); // Dados inválidos para falhar na validação
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Empty(result.Qr_data);
        }

        [Fact]
        public async Task Handle_DeveRetornarQrData_QuandoComandoExecutadoComSucesso()
        {
            // Arrange
            var orderItemInput = new OrderItemInput
            {
                Title = "Title teste",
                Description = "Description Teste",
                Unit_price = 10,
                Quantity = 1,
                Unit_measure = "",
                Total_amount = 10
            };
            var orderItensInput = new List<OrderItemInput>();
            orderItensInput.Add(orderItemInput);

            var orderInput = new OrderInput
            {
                External_reference = "External teste",
                Title = "Title teste",
                Description = "Description Teste",
                Expiration_date = "2024-12-31",
                Total_amount = 10,
                Items = orderItensInput
            };

            var pedido = new Pedido(Guid.NewGuid(), false, 0, 10);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 10, 10);
            var orderItem = new OrderItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem);
            var orderItems = new List<OrderItem>();
            orderItems.Add(orderItem);
            var mercadoPagoOrder = new MercadoPagoOrder(pedido, orderItems, "Teste");

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<MercadoPagoOrder>(It.IsAny<OrderInput>())).Returns(mercadoPagoOrder);

            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            mercadoPagoUseCaseMock.Setup(m => m.GerarQRCode(It.IsAny<MercadoPagoOrder>())).ReturnsAsync("qr_data_teste");

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(orderInput);
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("qr_data_teste", result.Qr_data);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoExcecaoDeDominioOcorrer()
        {
            // Arrange
            var orderItemInput = new OrderItemInput
            {
                Title = "Title teste",
                Description = "Description Teste",
                Unit_price = 10,
                Quantity = 1,
                Unit_measure = "",
                Total_amount = 10
            };
            var orderItensInput = new List<OrderItemInput>();
            orderItensInput.Add(orderItemInput);

            var orderInput = new OrderInput
            {
                External_reference = "External teste",
                Title = "Title teste",
                Description = "Description Teste",
                Expiration_date = "2024-12-31",
                Total_amount = 10,
                Items = orderItensInput
            };

            var pedido = new Pedido(Guid.NewGuid(), false, 0, 10);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 10, 10);
            var orderItem = new OrderItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem);
            var orderItems = new List<OrderItem>();
            orderItems.Add(orderItem);
            var mercadoPagoOrder = new MercadoPagoOrder(pedido, orderItems, "Teste");

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<MercadoPagoOrder>(It.IsAny<OrderInput>())).Returns(mercadoPagoOrder);

            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            mercadoPagoUseCaseMock.Setup(m => m.GerarQRCode(It.IsAny<MercadoPagoOrder>())).ThrowsAsync(new DomainException("Erro de domínio"));

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(orderInput);
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuantoTiverErros()
        {
            // Arrange
            var orderInput = new OrderInput {};

            var mercadoPagoOrder = new MercadoPagoOrder {};

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<MercadoPagoOrder>(It.IsAny<OrderInput>())).Returns(mercadoPagoOrder);

            var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
            mercadoPagoUseCaseMock.Setup(m => m.GerarQRCode(It.IsAny<MercadoPagoOrder>())).ThrowsAsync(new DomainException("Erro de domínio"));

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new GerarQRCommand(orderInput);
            var handler = new GerarQRCommandHandler(mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<GerarQROutput>(result);
            Assert.Equal(string.Empty, result.Qr_data);
        }
    }
}
