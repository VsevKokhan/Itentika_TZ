using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Core.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public EventTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }
    public Guid IncidentId { get; set; }
}