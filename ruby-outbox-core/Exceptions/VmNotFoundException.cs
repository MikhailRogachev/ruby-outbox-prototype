namespace ruby_outbox_core.Exceptions;

public class VmNotFoundException : OutboxServiceException
{
    public VmNotFoundException(Guid vmId, string message) : base(vmId: vmId, message: message)
    {
    }
}
