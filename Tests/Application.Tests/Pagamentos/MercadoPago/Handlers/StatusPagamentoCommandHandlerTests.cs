// using Application.Pagamentos.MercadoPago.Commands;
// using Application.Pagamentos.MercadoPago.Handlers;
// using Application.Pagamentos.MercadoPago.UseCases;
// using Application.Pedidos.UseCases;
// using Domain.Base.Communication.Mediator;
// using Domain.Base.DomainObjects;
// using Domain.Base.Messages.CommonMessages.Notifications;
// using Domain.MercadoPago;
// using Domain.Pedidos;
// using Moq;

// namespace Application.Tests.Pagamentos.MercadoPago.Handlers
// {
//     public class StatusPagamentoCommandHandlerTests
//     {
//         [Fact]
//         public async Task Handle_DeveRetornarFalse_QuandoValidacaoFalha()
//         {
//             // Arrange
//             var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
//             var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
//             var mediatorHandlerMock = new Mock<IMediatorHandler>();
//             var command = new StatusPagamentoCommand(0, ""); // Dados inválidos para falhar na validação
//             var handler = new StatusPagamentoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

//             // Act
//             var result = await handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.False(result);
//         }

//         [Fact]
//         public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucesso()
//         {
//             // Arrange
//             var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
//             var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
//             var mediatorHandlerMock = new Mock<IMediatorHandler>();
//             var command = new StatusPagamentoCommand(123, "payment");
//             var handler = new StatusPagamentoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

//             var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "closed", External_reference = Guid.NewGuid().ToString() };
//             mercadoPagoUseCaseMock.Setup(m => m.PegaStatusPedido(It.IsAny<long>())).ReturnsAsync(mercadoPagoOrderStatus);

//             // Act
//             var result = await handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.True(result);
//             pedidoUseCaseMock.Verify(p => p.TrocaStatusPedido(It.IsAny<Guid>(), PedidoStatus.Pago), Times.Once);
//         }

//         [Fact]
//         public async Task Handle_DevePublicarNotificacao_QuandoExcecaoDeDominioOcorrer()
//         {
//             // Arrange
//             var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
//             var mercadoPagoUseCaseMock = new Mock<IMercadoPagoUseCase>();
//             var mediatorHandlerMock = new Mock<IMediatorHandler>();
//             var command = new StatusPagamentoCommand(123, "payment");
//             var handler = new StatusPagamentoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, mercadoPagoUseCaseMock.Object);

//             mercadoPagoUseCaseMock.Setup(m => m.PegaStatusPedido(It.IsAny<long>())).ThrowsAsync(new DomainException("Erro de domínio"));

//             // Act
//             var result = await handler.Handle(command, CancellationToken.None);

//             // Assert
//             Assert.False(result);
//             mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
//         }

//     }
// }
