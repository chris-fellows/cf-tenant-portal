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

        public async Task<IViewComponentResult> InvokeAsync(List<PropertyOwnerVM> propertyOwners)
        {
            var model = new PropertyOwnerListVM()
            {
                PropertyOwners = propertyOwners
            };
            return await Task.FromResult((IViewComponentResult)View("PropertyOwnerList", model));
        }
    }
}
