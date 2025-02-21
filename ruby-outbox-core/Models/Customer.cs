namespace ruby_outbox_core.Models;

public class Customer : Base
{
    public ICollection<Vm> Vms { get; set; } = new List<Vm>();

    private Customer() { }
}
