using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Pagamentos.MercadoPago.Commands;
using Application.Pagamentos.MercadoPago.Handlers;
using Application.Pagamentos.MercadoPago.Gateways;
using Application.Pagamentos.MercadoPago.UseCases;
using Domain.MercadoPago;
using Infra.Pagamento.MercadoPago.Repository;
using Domain.PedidosQR.Interface;
using Infra.Pagamento.PedidosQR.Repository;
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
            
            // Pagamento
            services.AddTransient<IRequestHandler<StatusPagamentoFakeCommand, bool>, StatusPagamentoFakeCommandHandler>();
            services.AddTransient<IRequestHandler<StatusPagamentoCommand, bool>, StatusPagamentoCommandHandler>();
            services.AddTransient<IRequestHandler<GerarQRCommand, bool>, GerarQRCommandHandler>();
            services.AddTransient<IRequestHandler<BuscarQRCommand, GerarQROutput>, BuscarQRCommandHandler>();
            services.AddScoped<IMercadoPagoUseCase, MercadoPagoUseCase>();
            services.AddScoped<IMercadoPagoRepository, MercadoPagoRepository>();
            services.AddScoped<IPedidosQRRepository, PedidosQRRepository>();

        }
    }
}
