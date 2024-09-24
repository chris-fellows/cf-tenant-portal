using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class PropertyListCompViewComponent : ViewComponent
    {        
        public PropertyListCompViewComponent()
        {            
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(PropertyListVM propertyList)
        {            
            return await Task.FromResult((IViewComponentResult)View("PropertyList", propertyList));
        }

        //public async Task<IViewComponentResult> InvokeAsync(List<PropertyBasicVM> properties)
        //{           
        //    var model = new PropertyListVM()
        //    {
        //        Properties = properties
        //    };
        //    return await Task.FromResult((IViewComponentResult)View("PropertyList", model));
        //}
    }
}
