using Application.Pagamentos.MercadoPago.Boundaries;
using Application.Pagamentos.MercadoPago.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
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
            Summary = "Gerar QR Code do mercado pago",
            Description = "Endpoint responsavel por gerar QR Code do mercado pago")]
        [SwaggerResponse(200, "Retorna o qr_data", typeof(GerarQROutput))]
        [SwaggerResponse(400, "Caso não seja preenchido todos os campos obrigatórios")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        [HttpPost]
        [Route("GerarQR")]
        public async Task<IActionResult> QRMercadoPago([FromBody]OrderInput input)
        {
            var command = new GerarQRCommand(input);
            var response = await _mediatorHandler.EnviarComando<GerarQRCommand, GerarQROutput>(command);

            if (OperacaoValida())
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());
            }
        }

    }
}
