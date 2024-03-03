using API.Setup;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;
using Domain.Configuration;
using Infra.RabbitMQ;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Domain.RabbitMQ;
using Infra.RabbitMQ.Consumers;
using Domain.PedidosQR;

var builder = WebApplication.CreateBuilder(args);

// Configurando LoggerFactory e criando uma inst�ncia de ILogger
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});
var logger = loggerFactory.CreateLogger<Program>();

// Add services to the container.
builder.Services.AddControllers();

string secret = "";

if (builder.Environment.IsProduction())
{
    logger.LogInformation("Ambiente de Producao detectado.");

    builder.Configuration.AddAmazonSecretsManager("us-west-2", "pagamento-secret");

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}
else
{
    logger.LogInformation("Ambiente de Desenvolvimento/Local detectado.");

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

builder.Services.Configure<Secrets>(builder.Configuration);

var dynamoLocalOptions = new DynamoLocalOptions();
builder.Configuration.GetSection("DynamoLocal").Bind(dynamoLocalOptions);
builder.Services.AddSingleton(dynamoLocalOptions);

// builder.Services.AddAuthenticationJWT(secret);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<RabbitMQModelFactory>();
builder.Services.AddSingleton(serviceProvider =>
{
    var modelFactory = serviceProvider.GetRequiredService<RabbitMQModelFactory>();
    return modelFactory.CreateModel();
});
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddHostedService<PedidoConfirmadoSubscriber>();

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UsePathBase(new PathString("/pagamento"));
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
