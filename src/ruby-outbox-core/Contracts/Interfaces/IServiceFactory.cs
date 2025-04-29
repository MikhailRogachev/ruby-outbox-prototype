namespace ruby_outbox_core.Contracts.Interfaces;

/// <summary>
/// This interface includes functions for services that create new instances of 
/// eventhandles based on specified parameters.
/// </summary>
public interface IServiceFactory
{
    /// <summary>
    /// This function creates the new instance of eventhandler base on event name.
    /// </summary>
    /// <param name="eventName">Event Name</param>
    /// <returns>
    /// Tuple
    ///     Instance? - instance of the event handler
    ///     Type?     - event type 
    /// </returns>
    (object? Instance, Type? InstanceType) GetServiceInstance(string eventName);

    /// <summary>
    /// This function creates the new instance of eventhandler base on event type.
    /// </summary>
    /// <param name="eventType">Event Type</param>
    /// <returns>
    /// Tuple
    ///     Instance? - instance of the event handler
    ///     Type?     - event type 
    /// </returns>
    (object? Instance, Type? InstanceType) GetServiceInstance(Type eventType);
}
