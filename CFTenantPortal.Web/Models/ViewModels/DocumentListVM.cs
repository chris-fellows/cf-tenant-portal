namespace CFTenantPortal.Models
{
    public class DocumentListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();

        public DocumentFilterVM Filter { get; set; }
    }
}
