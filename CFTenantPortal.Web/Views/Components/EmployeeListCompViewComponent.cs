using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class EmployeeListCompViewComponent : ViewComponent
    {
        public EmployeeListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(EmployeeListVM employeeList)
        {            
            return await Task.FromResult((IViewComponentResult)View("EmployeeList", employeeList));
        }

        //public async Task<IViewComponentResult> InvokeAsync(List<EmployeeBasicVM> employees)
        //{
        //    var model = new EmployeeListVM()
        //    {
        //        Employees = employees
        //    };
        //    return await Task.FromResult((IViewComponentResult)View("EmployeeList", model));
        //}
    }
}
