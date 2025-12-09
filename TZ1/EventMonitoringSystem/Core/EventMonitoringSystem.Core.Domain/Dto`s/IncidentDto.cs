using EventMonitoringSystem.Core.Domain.Enums;

namespace EventMonitoringSystem.Core.Domain.Dto_s;

public class IncidentDto
{
    public Guid Id { get; set; }
    public IncidentTypeEnum TypeEnum { get; set; }
    public DateTime Time { get; set; }

    public ICollection<EventDto> Events { get; set; } = new List<EventDto>();
}