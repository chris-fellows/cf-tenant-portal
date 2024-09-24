using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class IssueListCompViewComponent :ViewComponent 
    {
        public IssueListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(IssueListVM issueList)
        {         
            return await Task.FromResult((IViewComponentResult)View("IssueList", issueList));
        }

        //public async Task<IViewComponentResult> InvokeAsync(List<IssueBasicVM> issues, bool allowCreate)
        //{
        //    var model = new IssueListVM()
        //    {
        //        AllowCreate = allowCreate,
        //        Issues = issues
        //    };
        //    return await Task.FromResult((IViewComponentResult)View("IssueList", model));
        //}
    }
}
