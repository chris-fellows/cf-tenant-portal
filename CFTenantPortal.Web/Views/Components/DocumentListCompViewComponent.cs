using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class DocumentListCompViewComponent : ViewComponent
    {
        public DocumentListCompViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<DocumentBasicVM> documents)
        {
            var model = new DocumentListVM()
            {
                Documents = documents
            };
            return await Task.FromResult((IViewComponentResult)View("DocumentList", model));
        }
    }
}
