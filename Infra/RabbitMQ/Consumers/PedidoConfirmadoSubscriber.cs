﻿using System.Text;
using Domain.Pedidos;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Application.Pagamentos.MercadoPago.Commands;
using Domain.RabbitMQ;
using Domain.Configuration;
using Microsoft.Extensions.Options;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoConfirmadoSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _nomeDaFila;
        private IModel _channel;

        public PedidoConfirmadoSubscriber(
            IServiceScopeFactory scopeFactory,
            IOptions<Secrets> options,
            IModel model)
        {
            _scopeFactory = scopeFactory;
            _nomeDaFila = options.Value.QueuePedidoConfirmado;
            var nomeExchange = options.Value.ExchangePedidoConfirmado;

            _channel = model;
            _channel.ExchangeDeclare(exchange: nomeExchange, type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: _nomeDaFila, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _nomeDaFila, exchange: nomeExchange, routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => { InvokeReceivedEvent(ModuleHandle, ea); };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        protected void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediatorHandler = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Pedido pedido;
                try
                {
                    pedido = JsonSerializer.Deserialize<Pedido>(message) ?? new Pedido();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro deserializar Pedido", ex);
                }
;
                var command = new GerarQRCommand(pedido);
                mediatorHandler.EnviarComando<GerarQRCommand, bool>(command).Wait();
            }
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();

            base.Dispose();
        }
    }
}
