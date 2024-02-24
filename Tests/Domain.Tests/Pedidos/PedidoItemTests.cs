using Domain.Pedidos;

namespace Domain.Tests.Pedidos
{
    public class PedidoItemTests
    {
        [Fact]
        public void DeveConstruirVazio_AoChamarConstrutor()
        {
            var pedidoItem = new PedidoItem();

            Assert.Empty(pedidoItem.ProdutoNome);
        }
    }
}
