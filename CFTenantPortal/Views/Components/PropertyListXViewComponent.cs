using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class PropertyListXViewComponent : ViewComponent
    {        
        public PropertyListXViewComponent()
        {            
            int xxx = 1000;
        }      

        public async Task<IViewComponentResult> InvokeAsync(List<PropertyBasicVM> properties)
        {           
            var model = new PropertyListVM()
            {
                Properties = properties
            };
            return await Task.FromResult((IViewComponentResult)View("PropertyListTest", model));
        }
    }
}
