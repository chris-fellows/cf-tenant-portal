using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Services
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly IAccountTransactionService _accountTransactionService;
        private readonly IAccountTransactionTypeService _accountTransactionTypeService;
        private readonly IMessageService _messageService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTypeService _messageTypeService;

        public PaymentRequestService(IAccountTransactionService accountTransactionService,
                                    IAccountTransactionTypeService accountTransactionTypeService,
                                    IMessageService messageService,
                                    IMessageTemplateService messageTemplateService,
                                    IMessageTypeService messageTypeService)
        {
            _accountTransactionService = accountTransactionService;
            _accountTransactionTypeService = accountTransactionTypeService;
            _messageService = messageService;
            _messageTemplateService = messageTemplateService;
            _messageTypeService = messageTypeService;
        }

        public async Task CreateManagementFeesRequest(PropertyOwner propertyOwner, Property property, double amount, string reference)
        {
            // Create account transaction
            var accountTransactionType = _accountTransactionTypeService.GetAll().First(t => t.TransactionType == Enums.AccountTransactionTypes.ManagemmentFeesRequest);

            var accountTransaction = new AccountTransaction()
            {
                CreatedDateTime = DateTimeOffset.UtcNow,
                PropertyId = property.Id,
                Reference = reference,
                TypeId = accountTransactionType.Id,
                Value = amount
            };

            await _accountTransactionService.AddAsync(accountTransaction);

            // Create message
            var messageType = _messageTypeService.GetAll().First(t => t.Description == "Management Fees Request");

            // Get message template
            var messageTemplate = await _messageTemplateService.GetByIdAsync(messageType.DefaultTemplateId);

            var messageText = messageTemplate.Text
                        .Replace("{PropertyOwner.Name}", propertyOwner.Name)
                        .Replace("{Property.Address}", property.Address.ToString());

            var message = new Message()
            {
                CreatedDateTime= accountTransaction.CreatedDateTime,
                PropertyId = property.Id,
                PropertyOwnerId = propertyOwner.Id,
                MessageTypeId = messageType.Id,
                Text = messageText                
            };
            await _messageService.AddAsync(message);
        }
    }
}
