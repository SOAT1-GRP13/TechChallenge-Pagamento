namespace Application.Pagamentos.MercadoPago.Boundaries
{
    public class OrderInput 
    {
        public OrderInput()
        {
            External_reference = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Expiration_date = string.Empty;
            Total_amount = 0;
            Items = new List<OrderItemInput>();

        }

        public string External_reference { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Expiration_date { get; set; }
        public decimal Total_amount { get; set; }
        public List<OrderItemInput> Items { get; set; }
    }

}