namespace ruby_outbox_core.Exceptions;

public class VmNotFoundException : Exception
{
    public VmNotFoundException(string message) : base(message)
    {
    }
}
