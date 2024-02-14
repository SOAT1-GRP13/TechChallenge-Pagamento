namespace Domain.Pedidos
{
    public class PedidoItem
    {
        public PedidoItem()
        {
            ProdutoNome = string.Empty;
        }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade,
         decimal valorUnitario, decimal valorTotal){
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
         }

        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }

    }
}
