using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Views.Components
{
    public class DocumentListXViewComponent : ViewComponent
    {
        public DocumentListXViewComponent()
        {
            int xxx = 1000;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<DocumentBasicVM> documents)
        {
            var model = new DocumentListVM()
            {
                Documents = documents
            };
            return await Task.FromResult((IViewComponentResult)View("DocumentListTest", model));
        }
    }
}
