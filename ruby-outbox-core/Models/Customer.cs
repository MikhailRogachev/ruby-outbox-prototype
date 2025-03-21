using ruby_outbox_core.Contracts.Enums;

namespace ruby_outbox_core.Models;
/*{
  "MySecret": "azure-subscription-17bdc332-0a80-450f-b4b8-b0300c42498e",
  "KeyVaultConfig:KeyVaultName": "pvad-dev-weu-app-kv",
  "AzureConfig:TenantId": "9993c174-ed13-464f-8384-2e28f515ff6a",
  "AzureConfig:ClientSecret": "1hP8Q~uApXi7xaHj-7m6XvTbP9eetVA_TpBE8bCY",
  "AzureConfig:ClientId": "ccd16d25-d15e-4308-a7a5-5faa68f01010",
  "MyRegion": "westeurope",
  "ResourceGroup": "mikhail-rogachev-vms-rg"
}*/
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
