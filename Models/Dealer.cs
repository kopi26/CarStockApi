namespace CarStockApi.Models
{
    public class Dealer
    {
        public int DealerId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}
