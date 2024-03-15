using Domain.PedidosQR;

namespace Domain.Tests.PedidosQR
{
    public class PedidoQRTests
    {
        [Fact]
        public void DeveConstruirCorretamente_AoChamarConstrutor()
        {
            var pedidoId = Guid.NewGuid().ToString();
            var pedidoQr = new PedidoQR(pedidoId, "teste", string.Empty);


            Assert.Equal(pedidoId, pedidoQr.PedidoId);
            Assert.Equal("teste", pedidoQr.QrData);
            Assert.True(pedidoQr.Ttl > 0);
        }

        [Fact]
        public void DeveConstruirVazio_AoChamarConstrutor()
        {
            var pedidoQr = new PedidoQR();


            Assert.Empty(pedidoQr.PedidoId);
            Assert.Empty(pedidoQr.QrData);
            Assert.True(pedidoQr.Ttl == 0);
        }
    }
}