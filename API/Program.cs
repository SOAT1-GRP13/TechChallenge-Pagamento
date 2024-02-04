using API.Data;
using API.Setup;
using Infra.Pedidos;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using Domain.Configuration;
using Application.Pagamentos.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configurando LoggerFactory e criando uma instância de ILogger
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

builder.Services.Configure<Secrets>(builder.Configuration);

builder.Services.AddAutoMapper(typeof(PagamentosMappingProfile));
// builder.Services.AddAuthenticationJWT(secret);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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
