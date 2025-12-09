using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Data.Domain.Entities;

public class EventDao
{
    public Guid Id { get; set; }
    public EventTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }
    public Guid IncidentId { get; set; }
    public virtual IncidentDao Incident { get; set; }
}