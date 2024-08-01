using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class MessageListXViewComponent : ViewComponent
    {
        public MessageListXViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<MessageBasicVM> messages)
        {
            var model = new MessageListVM()
            {
                Messages = messages
            };
            return await Task.FromResult((IViewComponentResult)View("MessageListTest", model));
        }
    }
}
