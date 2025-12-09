using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Core.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public IncidentTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }
}