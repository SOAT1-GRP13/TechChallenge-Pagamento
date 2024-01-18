using Moq;
using Domain.Pedidos;
using Domain.Configuration;
using Infra.Pedidos.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infra.Tests.Pedidos.Repository
{
    public class PedidoRepositoryTests
    {
        private readonly Mock<IOptions<Secrets>> _mockOptions;
        private readonly Mock<ILogger<PedidoRepository>> _mockLogger;
        private readonly Secrets _secrets;

        public PedidoRepositoryTests()
        {
            _mockOptions = new Mock<IOptions<Secrets>>();
            _mockLogger = new Mock<ILogger<PedidoRepository>>();
            _secrets = new Secrets { Producao_url = "http://testurl.com" };

            _mockOptions.Setup(opt => opt.Value).Returns(_secrets);
        }

        [Fact]
        public async Task Atualizar_DeveLogarErro_QuandoRespostaApiNaoEhSucesso()
        {
            // Arrange

            var repository = new PedidoRepository(_mockOptions.Object, _mockLogger.Object);

            // Act
            await repository.Atualizar(new AtualizaStatus(Guid.NewGuid(), PedidoStatus.Pago));

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("404 Not Found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }
    }
}
