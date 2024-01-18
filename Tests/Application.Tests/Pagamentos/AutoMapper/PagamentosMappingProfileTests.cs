using Application.Pagamentos.AutoMapper;
using Application.Pagamentos.MercadoPago.Boundaries;
using AutoMapper;
using Domain.MercadoPago;

namespace Application.Tests.Pagamentos.AutoMapper
{
    public class PagamentosMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PagamentosMappingProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<PagamentosMappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void DeveMapearOrderItemInputParaMercadoPagoOrderCorretamente()
        {
            // Arrange
            var orderInput = new OrderInput()
            {
                External_reference = "External teste",
                Title = "Title teste",
                Description = "Description Teste",
                Expiration_date = string.Empty,
                Total_amount = 0,
                Items = new List<OrderItemInput>()
            };

            // Act
            var mercadoPagoOrder = _mapper.Map<MercadoPagoOrder>(orderInput);

            // Assert
            Assert.Equal(orderInput.External_reference, mercadoPagoOrder.External_reference);
            Assert.Equal(orderInput.Title, mercadoPagoOrder.Title);
        }

        [Fact]
        public void DeveMapearOrderItemInputParaOrderItemCorretamente()
        {
            // Arrange
            var orderItemInput = new OrderItemInput
            {
                Title = "Title teste",
                Description = "Description Teste",
                Unit_price = 0,
                Quantity = 0,
                Unit_measure = "",
                Total_amount = 0
        };

            // Act
            var orderItem = _mapper.Map<OrderItem>(orderItemInput);

            // Assert
            Assert.Equal(orderItemInput.Title, orderItem.Title);
            Assert.Equal(orderItemInput.Description, orderItem.Description);
        }
    }
}
