using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class AuditEventListCompViewComponent : ViewComponent
    {
        public AuditEventListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(AuditEventListVM auditEventList)
        {
            return await Task.FromResult((IViewComponentResult)View("AuditEventList", auditEventList));
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
