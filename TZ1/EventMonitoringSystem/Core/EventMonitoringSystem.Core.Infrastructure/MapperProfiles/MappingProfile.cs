using AutoMapper;
using EventMonitoringSystem.Core.Domain.Dto_s;
using EventMonitoringSystem.DB.Main.Domain.Entities;

namespace EventMonitoringSystem.Core.Infrastructure.MapperProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Incident, IncidentDto>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events));
        CreateMap<Event, EventDto>();
        CreateMap<Incident, IncidentDto>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events));

    }
}