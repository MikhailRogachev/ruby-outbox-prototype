using ruby_outbox_core.Contracts.Enums;

namespace ruby_outbox_core.Models;

public class Vm : Base
{
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public VmStatus? Status { get; set; } = VmStatus.NotStarted;
    public ICollection<CloudProcess> CloudProcesses { get; set; } = new List<CloudProcess>();
    private Vm() { }
    public Vm(Guid customerId) : base()
    {
        CustomerId = customerId;
        Comment = "Added new Vm";
    }




}
