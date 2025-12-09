using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.DB.Main.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public IncidentTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}