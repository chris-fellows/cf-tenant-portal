using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class EmployeeListXViewComponent : ViewComponent
    {
        public EmployeeListXViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<EmployeeBasicVM> employees)
        {
            var model = new EmployeeListVM()
            {
                Employees = employees
            };
            return await Task.FromResult((IViewComponentResult)View("EmployeeListTest", model));
        }
    }
}
