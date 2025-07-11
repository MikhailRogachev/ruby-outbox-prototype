﻿using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Events.CreateVm;

namespace ruby_outbox_core.Models;

/// <summary>
///     Represents a virtual machine (VM) entity associated with a customer. Provides methods 
///     to manage the VM creation process and its lifecycle.
/// </summary>
/// <remarks>
///     The <see cref="Vm"/> class encapsulates the state and operations related to the creation 
///     and management of a virtual machine. It includes properties for identifying the customer, 
///     tracking the VM's status, and managing related resources. 
///     The class also provides methods to transition the VM through various stages of its
///     creation process.
/// </remarks>
public class Vm : Base
{
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public VmStatus? Status { get; set; } = VmStatus.NotStarted;
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
        AddEvent(new StartVmCreation { VmId = Id, CustomerId = CustomerId!.Value });
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

    public void CreateAadLogin()
    {
        Status = VmStatus.CreateAadLogin;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new CreateAadLoginExtension { VmId = Id });
    }

    public void RunPowershellCommand()
    {
        Status = VmStatus.RunPowershellCommand;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new RunPowerShellCommand { VmId = Id });
    }

    public void CompleteVmCreation()
    {
        Status = VmStatus.Ready;
        UpdatedAt = DateTime.UtcNow;
        AddEvent(new CompleteCreateVmProcess { VmId = Id });
    }
}
