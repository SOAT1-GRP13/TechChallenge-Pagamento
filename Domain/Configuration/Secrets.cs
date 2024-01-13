namespace Domain.Configuration
{
    public class Secrets
    {
        public Secrets()
        {
            MercadoPagoUserId = string.Empty;
            AccesToken = string.Empty;
            Notification_url = string.Empty;
            External_Pos_Id = string.Empty;
            Producao_url = string.Empty;
        }

        public string MercadoPagoUserId { get; set; }
        public string AccesToken { get; set; }
        public string Notification_url { get; set; }
        public string External_Pos_Id { get; set; }
        public string Producao_url { get; set; }
    }
}