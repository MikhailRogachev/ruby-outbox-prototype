namespace ruby_outbox_core.Contracts.Interfaces.Services;

public interface IProcessResolver
{
    object Resolve(Type @type);
}
