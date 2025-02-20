namespace ruby_outbox_core.Models;

public class CloudProcess : Base
{
    public Guid? VmId { get; set; }
    public Vm? Vm { get; set; }
}
