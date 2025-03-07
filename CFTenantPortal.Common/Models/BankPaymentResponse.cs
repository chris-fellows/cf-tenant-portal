namespace CFTenantPortal.Models
{
    public class BankPaymentResponse 
    {
        public bool Authorised { get; set; }

        public string Code { get; set; } = String.Empty;
    }
}
