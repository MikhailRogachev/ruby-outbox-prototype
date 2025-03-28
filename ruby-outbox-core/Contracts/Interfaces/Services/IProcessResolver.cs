namespace ruby_outbox_core.Contracts.Interfaces.Services;

[Obsolete]
public interface IProcessResolver
{
    object Resolve(Type @type);
    Type ResolveType(Type @type);
}
