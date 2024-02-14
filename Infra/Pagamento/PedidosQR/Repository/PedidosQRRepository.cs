using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Domain.PedidosQR;
using Domain.PedidosQR.Interface;
using Microsoft.Extensions.Hosting;

namespace Infra.Pagamento.PedidosQR.Repository
{
    public class PedidosQRRepository : IPedidosQRRepository
    {
        private readonly IDynamoDBContext _dynamoDBContext;

        public PedidosQRRepository(IAmazonDynamoDB client, IHostEnvironment hostingEnv, DynamoLocalOptions options)
        {
            if (hostingEnv.IsDevelopment())
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = options.ServiceUrl
                };
                AmazonDynamoDBClient dynamoClient = new AmazonDynamoDBClient("Aksdjaksdhueiadqwert", "dshdajksdhajskdhasjkdhasjkdashkjqwertyu", clientConfig);
                _dynamoDBContext = new DynamoDBContext(dynamoClient);
            }
            else
            {
                _dynamoDBContext = new DynamoDBContext(client);
            }
        }
        public async Task SalvaPedidoQR(QrCodeDTO dto)
        {
            try
            {
                var pedidoQR = new PedidoQR(dto.PedidoId, dto.QRData);

                await _dynamoDBContext.SaveAsync(pedidoQR);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}