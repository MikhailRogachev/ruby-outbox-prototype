namespace ruby_outbox_core.Models;

public class Customer : Base
{
    public virtual ICollection<Vm> Vms { get; set; } = new List<Vm>();

    public Customer() { }
}
