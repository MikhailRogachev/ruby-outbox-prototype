namespace ruby_outbox_core.AzureRequests
{
    public class AzureInlineCommandRequest : AzureBaseRequest
    {
        public string CustomerSecret { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
