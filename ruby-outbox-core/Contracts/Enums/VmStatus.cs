namespace ruby_outbox_core.Contracts.Enums;

public enum VmStatus
{
    NotStarted,
    Creating,
    CreatingFailed,
    Ready,
    Personalizing,
    AdminPersonolized,
    UserPersonolized,
    PersonolizingFailed,
    Deleting,
    DeletingFailed
}
