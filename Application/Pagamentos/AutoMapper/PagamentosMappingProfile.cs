using Application.Pagamentos.MercadoPago.Boundaries;
using AutoMapper;
using Domain.MercadoPago;

namespace Application.Pagamentos.AutoMapper
{
    public class PagamentosMappingProfile : Profile
    {
        public PagamentosMappingProfile()
        {
            CreateMap<OrderInput, MercadoPagoOrder>();
            CreateMap<OrderItemInput, OrderItem>();
        }
    }
}