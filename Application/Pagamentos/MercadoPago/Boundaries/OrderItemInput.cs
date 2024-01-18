namespace Application.Pagamentos.MercadoPago.Boundaries
{
    public class OrderItemInput
    {
        public OrderItemInput()
        {
            Title = string.Empty;
            Description = string.Empty;
            Unit_price = 0;
            Quantity = 0;
            Unit_measure = "";
            Total_amount = 0;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Unit_price { get; set; }
        public int Quantity { get; set; }
        public string Unit_measure { get; set; }
        public decimal Total_amount { get; set; }
    }
}