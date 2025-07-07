using ruby_outbox_core.Contracts.Enums;

namespace ruby_outbox_core.Models;

public class Customer : Base
{
    public virtual ICollection<Vm> Vms { get; set; } = new List<Vm>();
    public string ClientSecret { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string KeyVaultName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string KeyVaultSecret { get; set; } = string.Empty;
    public string ResourceGroup { get; set; } = string.Empty;
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public Customer() { }
}
