using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.DB.Main.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public EventTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }

    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; }
}