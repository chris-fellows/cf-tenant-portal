using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    /// <summary>
    /// Test payment gateway service. Specific car numbers can be added
    /// </summary>
    public class TestBankPaymentGatewayService : IBankPaymentGatewayService
    {
        private List<string> _authoriseCardNumbers = new List<string>();

        public void AddCardNumber(string number)
        {
            if (!_authoriseCardNumbers.Contains(number)) _authoriseCardNumbers.Add(number);
        }

        public void RemoveCardNumber(string number)
        {
            if (_authoriseCardNumbers.Contains(number)) _authoriseCardNumbers.Remove(number);
        }

        public async Task<BankPaymentResponse> MakePaymentAsync(double amount, BankCard bankCard)
        {
            await Task.Delay(5000); // Random delay

            var response = _authoriseCardNumbers.Contains(bankCard.Number) ?
                        new BankPaymentResponse()
                        {
                            Authorised = true,
                            Code = "1234"
                        } :
                        new BankPaymentResponse()
                        {
                            Authorised = false
                        };
            return response;
        }
    }
}
