using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Core.Domain.interfaces;

public interface IEventPublisher
{
    Task PublishAsync(EventTypeEnum typeEnum);
}