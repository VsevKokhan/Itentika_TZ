using AutoMapper;
using EventMonitoringSystem.Core.Domain.Entities;
using EventMonitoringSystem.Data.Domain.Entities;

namespace EventMonitoringSystem.Core.Infrastructure.MapperProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventDao, Event>();
        CreateMap<Event, EventDao>();

        CreateMap<IncidentDao, Incident>();
        CreateMap<Incident, IncidentDao>();
    }
}