using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Domain.PedidosQR;
using Domain.PedidosQR.Interface;

namespace Infra.Pagamento.PedidosQR.Repository
{
    public class PedidosQRRepository : IPedidosQRRepository
    {
        private readonly IDynamoDBContext _dynamoDBContext;

        public PedidosQRRepository(IAmazonDynamoDB client, DynamoLocalOptions options)
        {
            _dynamoDBContext = new DynamoDBContext(client);
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // if (!string.IsNullOrEmpty(env) && env == "Development")
            // {
            //     AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
            //     {
            //         ServiceURL = options.ServiceUrl
            //     };
            //     AmazonDynamoDBClient dynamoClient = new AmazonDynamoDBClient("Aksdjaksdhueiadqwert", "dshdajksdhajskdhasjkdhasjkdashkjqwertyu", clientConfig);
            //     _dynamoDBContext = new DynamoDBContext(dynamoClient);
            // }
            // else
            // {
            //     _dynamoDBContext = new DynamoDBContext(client);
            // }
        }
        public async Task SalvaPedidoQR(QrCodeDTO dto)
        {
            var pedidoQR = new PedidoQR(dto.PedidoId, dto.QRData);

            await _dynamoDBContext.SaveAsync(pedidoQR);

        }

        public async Task<QrCodeDTO> BuscaPedidoQr(string pedidoId)
        {
            var pedidoQR = await _dynamoDBContext.LoadAsync<PedidoQR>(pedidoId);

            if (pedidoQR != null)
                return new QrCodeDTO(pedidoQR.QrData, pedidoId);

            return new QrCodeDTO();
        }
    }
}