using Moq;
using MediatR;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Domain.Base.Communication.Mediator;
using Application.Pagamentos.MercadoPago.Commands;
using Domain.Base.Messages.CommonMessages.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Application.Pagamentos.MercadoPago.Boundaries;
using Microsoft.Extensions.Hosting;

namespace API.Tests.Controllers
{
    public class MercadoPagoControllerTests
    {
        [Fact]
        public async Task WebHookMercadoPago_DeveRetornarOk_QuandoComandoBemSucedido()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var mercadoPagoController = new MercadoPagoController(
                domainNotificationHandler,
                mediatorHandlerMock.Object
            );

            long id = 123;
            string topic = "payment";
            mediatorHandlerMock.Setup(m => m.EnviarComando<StatusPagamentoCommand, bool>(
                It.IsAny<StatusPagamentoCommand>())).ReturnsAsync(true);

            // Act
            var result = await mercadoPagoController.WebHookMercadoPago(id, topic);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task WebHookMercadoPago_DeveRetornarBadRequest_QuandoComandoFalha()
        {
            // Arrange
            var serviceProvider = new TestStartup().ConfigureServices(new ServiceCollection());

            // Obtenha uma instância real de DomainNotificationHandler do contêiner
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();
            var mediatorHandler = serviceProvider.GetRequiredService<IMediatorHandler>();

            var mercadoPagoController = new MercadoPagoController(
                domainNotificationHandler,
                mediatorHandler
            );

            // Act
            var result = await mercadoPagoController.WebHookMercadoPago(0, "");

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            var mensagensErro = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Contains("'Id' deve ser informado.", mensagensErro);
            Assert.Contains("Action é obrigatório", mensagensErro);
            Assert.Contains("Topic é obrigatório", mensagensErro);
        }

        [Fact]
        public async Task FakeWebhook_DeveRetornarOk_QuandoComandoExecutadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = new DomainNotificationHandler();
            var mercadoPagoController = new MercadoPagoController(domainNotificationHandler, mediatorHandlerMock.Object);

            mediatorHandlerMock.Setup(m => m.EnviarComando<StatusPagamentoFakeCommand, bool>(It.IsAny<StatusPagamentoFakeCommand>()))
                               .ReturnsAsync(true);

            // Act
            var result = await mercadoPagoController.FakeWebhook(Guid.NewGuid(), "sucesso", "closed");

            // Assert
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task BuscaQR_DeveRetornarOk_QuandoComandoExecutadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = new DomainNotificationHandler();
            var mercadoPagoController = new MercadoPagoController(domainNotificationHandler, mediatorHandlerMock.Object);

            var guidResult = Guid.NewGuid();
            var gerarQrResult = new GerarQROutput("sucesso", guidResult.ToString());

            mediatorHandlerMock.Setup(m => m.EnviarComando<BuscarQRCommand, GerarQROutput>(It.IsAny<BuscarQRCommand>()))
                               .ReturnsAsync(gerarQrResult);

            // Act
            var result = await mercadoPagoController.BuscaQR(guidResult);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(gerarQrResult, okResult?.Value);
        }

        [Fact]
        public async Task BuscaQR_DeveRetornarBadRequest_QuandoComandoFalhaNaValidacao()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = new DomainNotificationHandler();
            var mercadoPagoController = new MercadoPagoController(domainNotificationHandler, mediatorHandlerMock.Object);

            mediatorHandlerMock.Setup(m => m.EnviarComando<BuscarQRCommand, GerarQROutput>(It.IsAny<BuscarQRCommand>()))
                .Callback<BuscarQRCommand>(cmd =>
                {
                    if (!cmd.EhValido())
                    {
                        foreach (var error in cmd.ValidationResult.Errors)
                        {
                            domainNotificationHandler.Handle(new DomainNotification(error.PropertyName, error.ErrorMessage), CancellationToken.None);
                        }
                    }
                })
                .ReturnsAsync(new GerarQROutput());

            // Act
            var result = await mercadoPagoController.BuscaQR(Guid.Empty);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            var mensagensErro = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Contains("PedidoId é obrigatório", mensagensErro);
        }
    }
}
