namespace ruby_outbox_core.Contracts.Options;

public class PersonalSettingsConfig
{
    public string Region { get; set; } = string.Empty;
    public string ResourceGroup { get; set; } = string.Empty;
    public string PersonalSecret { get; set; } = string.Empty;
}
