namespace ruby_outbox_core.Contracts.Enums;

public enum OutboxMessageStatus
{
    Ini,
    Locked,
    Repeat,
    Error
}
