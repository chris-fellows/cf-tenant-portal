using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class IssueListXViewComponent :ViewComponent 
    {
        public IssueListXViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<IssueBasicVM> issues)
        {
            var model = new IssueListVM()
            {
                Issues = issues
            };
            return await Task.FromResult((IViewComponentResult)View("IssueListTest", model));
        }
    }
}
