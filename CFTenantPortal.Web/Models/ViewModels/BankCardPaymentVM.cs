namespace CFTenantPortal.Models
{
    public class BankCardPaymentVM
    {
        public string Number { get; set; } = String.Empty;

        public string AccountName { get; set; } = String.Empty;

        public string Expiry { get; set; } = String.Empty;

        public string Code { get; set; } = String.Empty;

        public double Amount { get; set; }
    }
}
