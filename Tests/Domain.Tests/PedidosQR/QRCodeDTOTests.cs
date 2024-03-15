using Domain.PedidosQR;

namespace Domain.Tests.PedidosQR
{
    public class QRCodeDTOTests
    {
        [Fact]
        public void DeveConstruirCorretamente_AoChamarConstrutor()
        {
            var qrCodeDTO = new QrCodeDTO("teste", "teste", string.Empty);


            Assert.Equal("teste", qrCodeDTO.PedidoId);
            Assert.Equal("teste", qrCodeDTO.QRData);
        }

        [Fact]
        public void DeveConstruirVazio_AoChamarConstrutor()
        {
            var pedidoQr = new QrCodeDTO();


            Assert.Empty(pedidoQr.PedidoId);
            Assert.Empty(pedidoQr.QRData);
        }
    }
}