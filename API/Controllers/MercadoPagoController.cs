using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Pedidos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [SwaggerTag("Endpoints relacionados ao mercado pago")]
    public class MercadoPagoController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;

        public MercadoPagoController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [SwaggerOperation(
            Summary = "Webhook mercado Pago",
            Description = "Endpoint responsavel por receber um evento do mercado pago")]
        [SwaggerResponse(200, "Retorna OK após alterar o status")]
        [SwaggerResponse(400, "Caso não seja preenchido todos os campos obrigatórios")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        [HttpPost]
        [Route("Webhook")]
        public async Task<IActionResult> WebHookMercadoPago([FromQuery] long id, [FromQuery] string topic)
        {
            var command = new StatusPagamentoCommand(id, topic);
            await _mediatorHandler.EnviarComando<StatusPagamentoCommand, bool>(command);

            if (OperacaoValida())
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());
            }
        }

        [SwaggerOperation(
            Summary = "Webhook fake mercado Pago",
            Description = "Endpoint responsavel por receber um evento simulando o mercado pago, para considerar como pago o status deve ser igual a closed, caso contraria será pedido recusado")]
        [SwaggerResponse(200, "Retorna OK após alterar o status")]
        [SwaggerResponse(400, "Caso não seja preenchido todos os campos obrigatórios")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        [HttpPost]
        [Route("FakeWebhook")]
        public async Task<IActionResult> FakeWebhook([FromQuery] Guid id, [FromQuery] string topic, [FromQuery] string status)
        {
            var command = new StatusPagamentoFakeCommand(id, topic, status);
            await _mediatorHandler.EnviarComando<StatusPagamentoFakeCommand, bool>(command);

            if (OperacaoValida())
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());
            }
        }

        [HttpGet("BuscaQR/{id}")]
        [SwaggerOperation(
            Summary = "Busca QR code pelo pedido id",
            Description = "Retorna QR code e pedido id.")]
        [SwaggerResponse(200, "Retorna o qr com pedido id", typeof(GerarQROutput))]
        [SwaggerResponse(404, "Caso não tenha pedido com o id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> BuscaQR([FromRoute, SwaggerRequestBody("uuid do pedido")] Guid id)
        {
            var command = new BuscarQRCommand(id);
            var response = await _mediatorHandler.EnviarComando<BuscarQRCommand, GerarQROutput>(command);

            if (OperacaoValida())
            {
                if (string.IsNullOrEmpty(response.Qr_data))
                    return NotFound();

                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());
            }

        }

    }
}
