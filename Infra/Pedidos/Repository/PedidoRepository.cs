using System.Text.Json;
using Domain.Configuration;
using Domain.Pedidos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infra.Pedidos.Repository
{
    public class PedidoRepository : IPedidoRepository
    {

        private readonly Secrets _secrets;
        private readonly ILogger _logger;

        public PedidoRepository(IOptions<Secrets> options, ILogger<PedidoRepository> logger)
        {
            _secrets = options.Value;
            _logger = logger;
        }

        public async Task Atualizar(AtualizaStatus pedido)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_secrets.Producao_url)
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "/producao/Pedidos/atualizar-status-pedido");
            var content = new StringContent(JsonSerializer.Serialize(pedido), null, "application/json");
            request.Content = content;
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
