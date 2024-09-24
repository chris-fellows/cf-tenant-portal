using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class MessageListCompViewComponent : ViewComponent
    {
        public MessageListCompViewComponent()
        {
            int xxx = 1000;
        }
        public async Task<IViewComponentResult> InvokeAsync(MessageListVM messageList)
        {            
            return await Task.FromResult((IViewComponentResult)View("MessageList", messageList));
        }


        //public async Task<IViewComponentResult> InvokeAsync(List<MessageBasicVM> messages)
        //{
        //    var model = new MessageListVM()
        //    {
        //        Messages = messages
        //    };
        //    return await Task.FromResult((IViewComponentResult)View("MessageList", model));
        //}
    }
}
