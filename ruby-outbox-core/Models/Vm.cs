using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Events;

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

    public void StartVmCreation()
    {
        Status = VmStatus.Creating;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new StartVmCreation { VmId = Id });
    }

    public void CreateNic()
    {
        Status = VmStatus.CreateNic;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new CreateNic { VmId = Id });
    }

    public void CreateVmResource()
    {
        Status = VmStatus.CreateVmResource;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new CreateVmResource { VmId = Id });
    }
}
