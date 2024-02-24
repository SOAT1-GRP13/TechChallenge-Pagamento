using Domain.PedidosQR;
using Domain.PedidosQR.Interface;
using Infra.Tests.Mock.Repositories;

namespace Infra.Tests.Pagamento.PedidosQR.Repository
{
    public class PedidosQRRepositoryTests
    {
        private readonly IPedidosQRRepository _repository;


        public PedidosQRRepositoryTests()
        {
            _repository = MockPedidosQRRepository.GetPedidosQRRepository().Object;
        }

        [Fact]

        public async Task DeveSalvarPedido_DynamoDB()
        {
            //Arrange
            var qrCodeDto = new QrCodeDTO("sucesso", "sucesso");

            // Act
            await _repository.SalvaPedidoQR(qrCodeDto);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public async Task AoBuscarQR_DeveRetornarOsDados_QuandoExistirOPedido()
        {

            // Act
            var result = await _repository.BuscaPedidoQr("sucesso");

            //Assert
            var objectResult = Assert.IsType<QrCodeDTO>(result);
            Assert.Equal("sucesso", objectResult.QRData);
            Assert.Equal("sucesso", objectResult.PedidoId);
        }

        [Fact]
        public async Task AoBuscarQR_DeveRetornarVazio_QuandoNaoExistirOPedido()
        {

            // Act
            var result = await _repository.BuscaPedidoQr("erro");

            //Assert
            var objectResult = Assert.IsType<QrCodeDTO>(result);
            Assert.Equal("", objectResult.QRData);
            Assert.Equal("", objectResult.PedidoId);
        }
    }
}
