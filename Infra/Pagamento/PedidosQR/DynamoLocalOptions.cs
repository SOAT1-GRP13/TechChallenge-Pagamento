namespace Infra.Pagamento.PedidosQR
{
    public class DynamoLocalOptions 
    {
        public DynamoLocalOptions()
        {
            ServiceUrl = string.Empty;
        }

        public string ServiceUrl {get;set;}
    }
}