using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class PropertyGroupListCompViewComponent : ViewComponent
    {
        public PropertyGroupListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<PropertyGroupVM> propertyGroups)
        {
            var model = new PropertyGroupListVM()
            {
                PropertyGroups = propertyGroups
            };
            return await Task.FromResult((IViewComponentResult)View("PropertyGroupList", model));
        }
    }
}