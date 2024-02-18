﻿using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.DomainObjects;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.MercadoPago;
using Domain.RabbitMQ;
using Moq;

namespace Application.Tests.Pagamentos.MercadoPago.Handlers
{
    public class StatusPagamentoFakeCommandHandlerTests
    {
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly RabbitMQOptions _rabbitMQOptions;
        private readonly StatusPagamentoFakeCommandHandler _handler;
        public StatusPagamentoFakeCommandHandlerTests()
        {
            _rabbitMQOptions = new RabbitMQOptions();
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new StatusPagamentoFakeCommandHandler(
                _mediatorHandlerMock.Object,
            _rabbitMQServiceMock.Object,
             _rabbitMQOptions);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoValidacaoFalha()
        {
            // Arrange
            var command = new StatusPagamentoFakeCommand(Guid.Empty, "", ""); // Dados inválidos para falhar na validação

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoComandoExecutadoComSucesso()
        {
            // Arrange
            var command = new StatusPagamentoFakeCommand(Guid.NewGuid(), "payment", "closed");
            var mercadoPagoOrderStatus = new MercadoPagoOrderStatus { Status = "closed", External_reference = Guid.NewGuid().ToString() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _rabbitMQServiceMock.Verify(r => r.PublicaMensagem(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}