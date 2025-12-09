using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Data.Domain.Entities;

public class IncidentDao
{
    public Guid Id { get; set; }
    public IncidentTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }
    public virtual ICollection<EventDao> Events { get; set; }
}