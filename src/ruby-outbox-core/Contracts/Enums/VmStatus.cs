namespace ruby_outbox_core.Contracts.Enums;

public enum VmStatus
{
    NotStarted,
    Creating,
    CreateNic,
    CreateVmResource,
    CreateAadLogin,
    RunPowershellCommand,
    Ready
}
