using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Controllers
{
    /// <summary>
    /// Bank payment controller.
    /// </summary>
    public class BankPaymentController : Controller
    {        
        private readonly IBankPaymentGatewayService _bankPaymentGatewayService;
        private readonly IBankPaymentService _bankPaymentService;

        public BankPaymentController(IBankPaymentGatewayService bankPaymentGatewayService,
                            IBankPaymentService bankPaymentService)
        {
            _bankPaymentGatewayService = bankPaymentGatewayService;
            _bankPaymentService = bankPaymentService;
        }

        public IActionResult BankCardPayment()
        {
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public IActionResult PaymentFailed()
        {
            return View();
        }

        /// <summary>
        /// Handles form to make bank payment
        /// </summary>
        /// <param name="bankCardPaymentVM"></param>
        /// <returns></returns>
        public IActionResult BankCardPaymentForm(BankCardPaymentVM bankCardPaymentVM)
        {
            var bankCard = new BankCard()
            {
                AccountName = bankCardPaymentVM.AccountName,
                Code = bankCardPaymentVM.Code,
                Expiry = bankCardPaymentVM.Expiry,
                Number = bankCardPaymentVM.Number
            };

            // Make payment
            var bankPaymentResponse = _bankPaymentGatewayService.MakePaymentAsync(bankCardPaymentVM.Amount, bankCard).Result;

            // Check response
            if (bankPaymentResponse.Authorised)
            {
                var bankPayment = new BankPayment()
                {                    
                    Authorised = true,
                    CardAccountName = bankCard.AccountName,
                    CardCode = bankCard.Code,
                    CardExpiry = bankCard.Expiry,
                    CardNumber = bankCard.Number.Substring(0, 4),
                    CreatedDateTime = DateTimeOffset.UtcNow,
                    ResponseCode = bankPaymentResponse.Code
                };
                _bankPaymentService.AddAsync(bankPayment).Wait();

                return RedirectToAction(nameof(PaymentSuccess));
            }
            else
            {
                var bankPayment = new BankPayment()
                {
                    Authorised = false,
                    CardAccountName = bankCard.AccountName,
                    CardCode = bankCard.Code,
                    CardExpiry = bankCard.Expiry,
                    CardNumber = bankCard.Number.Substring(0, 4),
                    CreatedDateTime = DateTimeOffset.UtcNow,
                    ResponseCode = bankPaymentResponse.Code
                };
                _bankPaymentService.AddAsync(bankPayment).Wait();

                return RedirectToAction(nameof(PaymentFailed));
            }
        }
    }
}
