using API.Setup;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Hosting;
using Domain.RabbitMQ;
using Domain.PedidosQR;
using Moq;
using Infra.RabbitMQ.Consumers;

namespace API.Tests
{
    public class TestStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var myConfiguration = new Dictionary<string, string>
            {
                {"AWS:Profile", "terraform"},
                {"AWS:Region", "us-west-2"}
            };

            var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(myConfiguration)
                    .Build();

            services.AddLogging();
            services.AddSingleton<IConfiguration>(configuration);

            var dynamoLocalOptions = new DynamoLocalOptions();
            configuration.GetSection("DynamoLocal").Bind(dynamoLocalOptions);
            services.AddSingleton(dynamoLocalOptions);

            var awsOptions = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            var rabbitMQServiceMock = new Mock<IRabbitMQService>();
            services.AddSingleton(rabbitMQServiceMock.Object);
            services.AddHostedService<PedidoConfirmadoSubscriber>();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            DependencyInjection.RegisterServices(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
