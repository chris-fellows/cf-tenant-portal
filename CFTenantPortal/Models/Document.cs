namespace CFTenantPortal.Models
{
    public class Document
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public byte[] Content { get; set; } = new byte[0];
    }
}
