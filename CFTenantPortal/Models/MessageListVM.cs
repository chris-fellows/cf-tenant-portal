namespace CFTenantPortal.Models
{
    public class MessageListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<MessageBasicVM> Messages { get; set; } = new List<MessageBasicVM>();
    }
}
