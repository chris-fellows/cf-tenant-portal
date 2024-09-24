using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class PropertyOwnerListCompViewComponent : ViewComponent
    {
        public PropertyOwnerListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(PropertyOwnerListVM propertyOwnerList)
        {            
            return await Task.FromResult((IViewComponentResult)View("PropertyOwnerList", propertyOwnerList));
        }

        //public async Task<IViewComponentResult> InvokeAsync(List<PropertyOwnerBasicVM> propertyOwners)
        //{
        //    var model = new PropertyOwnerListVM()
        //    {
        //        PropertyOwners = propertyOwners
        //    };
        //    return await Task.FromResult((IViewComponentResult)View("PropertyOwnerList", model));
        //}
    }
}
