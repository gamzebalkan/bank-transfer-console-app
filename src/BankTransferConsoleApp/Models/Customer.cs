namespace BankTransferConsoleApp.Models
{
    public class Customer
    {
        public string CustomerNo { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public string Currency { get; set; } = "USD";
    }
}