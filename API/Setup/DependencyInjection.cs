using Domain.Pedidos;
using Infra.Pedidos.Repository;
using Infra.Pedidos;
using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Pedidos.UseCases;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.Gateways;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.MercadoPago;
using Infra.MercadoPago.Repository;
using Application.Pagamentos.MercadoPago.Boundaries;

namespace API.Setup
{
    public static class DependencyInjection
    { 
        public static void RegisterServices(this IServiceCollection services)
        {
            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Domain Notifications 
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Pedidos
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoUseCase, PedidoUseCase>();
            
            // Pagamento
            services.AddTransient<IRequestHandler<StatusPagamentoCommand, bool>, StatusPagamentoCommandHandler>();
            services.AddTransient<IRequestHandler<GerarQRCommand, GerarQROutput>, GerarQRCommandHandler>();
            services.AddScoped<IMercadoPagoUseCase, MercadoPagoUseCase>();
            services.AddScoped<IMercadoPagoRepository, MercadoPagoRepository>();

        }
    }
}
