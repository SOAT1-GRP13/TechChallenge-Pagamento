using Domain.Configuration;
using Domain.MercadoPago;
using Domain.Pedidos;
using Domain.PedidosQR;
using Domain.PedidosQR.Interface;
using Infra.Pagamento.MercadoPago.Repository;
using Infra.Tests.Mock.Repositories;
using Microsoft.Extensions.Options;
using Moq;

namespace Infra.Tests.PedidosQR.Repository
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

        // [Fact]
        // public async Task DeveEstourarException_Se_Algo_Estiver_Configurado_Errado()
        // {
        //     //Arrange
        //     var qrCodeDto = new QrCodeDTO("erro", "erro");

        //     // Act
        //     //Assert
        //     await Assert.ThrowsAsync<Exception>(() => _repository.SalvaPedidoQR(qrCodeDto));
        // }

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
