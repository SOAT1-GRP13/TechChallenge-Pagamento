using System.Text.Json.Serialization;

namespace Domain.Pedidos
{
    public class AtualizaStatus
    {
        public AtualizaStatus(Guid idPedido, PedidoStatus status)
        {
            IdPedido = idPedido;
            Status = (int)status;
        }
        
        [JsonPropertyName("idPedido")]
        public Guid IdPedido { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}