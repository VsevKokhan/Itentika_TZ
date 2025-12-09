using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Core.Domain.Dto_s;

public class EventDto
{
    public Guid Id { get; set; }
    public EventTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }
}