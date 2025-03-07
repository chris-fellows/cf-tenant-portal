using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class PopupMessageCompViewComponent : ViewComponent
    {
        public PopupMessageCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {         
            return await Task.FromResult((IViewComponentResult)View("PopupMessage"));
        }
    }
}
