using AutoMapper;
using EventMonitoringSystem.Core.Application.Persistence;
using EventMonitoringSystem.Core.Domain.Dto_s;
using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;
using EventMonitoringSystem.DB.Main.Domain.Entities;
using EventMonitoringSystem.DB.Main.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventMonitoringSystem.Core.Infrastructure.Services;


public class IncidentService : IIncidentService
{
    private readonly IReadRepository<MonitoringDbContext> _readRepository;
    private readonly IWriteRepository<MonitoringDbContext> _writeRepository;
    private readonly ILogger<IncidentService> _logger;
    private readonly IMapper _mapper;

    public IncidentService(
        IReadRepository<MonitoringDbContext> readRepository,
        IWriteRepository<MonitoringDbContext> writeRepository,
        ILogger<IncidentService> logger,
        IMapper mapper
        )
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<List<IncidentDto>> GetAllIncidentsAsync()
    {
        var incidents = await _readRepository.DbContext.Incidents.Include(i => i.Events).ToListAsync();
        return _mapper.Map<List<IncidentDto>>(incidents);
    }
    
    public async Task ProcessAsync(EventDto eventDto)
    {
        _logger.LogInformation("Получено событие {EventId} типа {EventTypeEnum}",
            eventDto.Id, eventDto.TypeEnum);

        switch (eventDto.TypeEnum)
        {
            case EventTypeEnum.Type1:
                await ProcessSimpleTemplate(new Event {Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time}) ;
                break;

            case EventTypeEnum.Type2:
                await ProcessCompositeTemplate2(new Event {Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time} );
                break;

            case EventTypeEnum.Type3:
                await ProcessCompositeTemplate3(new Event {Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time} );
                break;

            default:
                _logger.LogWarning("Неизвестный тип события {TypeEnum}", eventDto.TypeEnum.ToString());
                break;
        }
    }

    private async Task ProcessSimpleTemplate(Event ev)
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            TypeEnum = IncidentTypeEnum.Type1,
            Time = DateTime.UtcNow,
            Events = new List<Event> { ev }
        };

        await _writeRepository.AddAsync(incident);
        await _writeRepository.SaveChangesAsync();

        _logger.LogInformation("Создан простой инцидент типа 1 на основе события {EventId}", ev.Id);
    }

    private async Task ProcessCompositeTemplate2(Event eventType2)
    {
        _logger.LogInformation("Запущена отложенная проверка Type2 для события {EventId}", eventType2.Id);

        _ = Task.Run(() => ProcessCompositeType2Delayed(eventType2));
    }

    private async Task ProcessCompositeType2Delayed(Event eventType2)
    {
        await Task.Delay(TimeSpan.FromSeconds(20));

        var type1Event = (await _readRepository.FindAsync<Event>(e =>
                e.TypeEnum == EventTypeEnum.Type1 &&
                e.Time >= eventType2.Time &&
                e.Time <= eventType2.Time.AddSeconds(20)))
            .OrderBy(e => e.Time)
            .FirstOrDefault();

        Incident incident;

        if (type1Event != null)
        {
            incident = new Incident
            {
                Id = Guid.NewGuid(),
                TypeEnum = IncidentTypeEnum.Type2,
                Time = DateTime.UtcNow,
                Events = new List<Event> { eventType2, type1Event }
            };

            _logger.LogInformation(
                "Создан составной инцидент типа 2 (Type2 в Type1) по событиям {A} и {B}",
                eventType2.Id, type1Event.Id);
        }
        else
        {
            incident = new Incident
            {
                Id = Guid.NewGuid(),
                TypeEnum = IncidentTypeEnum.Type1,
                Time = DateTime.UtcNow,
                Events = new List<Event> { eventType2 }
            };

            _logger.LogInformation(
                "Создан простой инцидент типа 1 на основе события Type2 {EventId}",
                eventType2.Id);
        }

        await _writeRepository.AddAsync(incident);
        await _writeRepository.SaveChangesAsync();
    }

    private async Task ProcessCompositeTemplate3(Event eventType3)
    {
        _logger.LogInformation("Запущена отложенная проверка Type3 для события {EventId}", eventType3.Id);

        _ = Task.Run(() => ProcessCompositeType3Delayed(eventType3));
    }

    private async Task ProcessCompositeType3Delayed(Event eventType3)
    {
        await Task.Delay(TimeSpan.FromSeconds(60));

        var incidentType2 = (await _readRepository.FindAsync<Incident>(i =>
                i.TypeEnum == IncidentTypeEnum.Type2 &&
                i.Time >= eventType3.Time &&
                i.Time <= eventType3.Time.AddSeconds(60)))
            .OrderBy(i => i.Time)
            .FirstOrDefault();

        Incident incident;

        if (incidentType2 != null)
        {
            incident = new Incident
            {
                Id = Guid.NewGuid(),
                TypeEnum = IncidentTypeEnum.Type3,
                Time = DateTime.UtcNow,
                Events = new List<Event> { eventType3 }
            };

            _logger.LogInformation(
                "Создан составной инцидент типа 3 (Type3 в Incident2) по событию {EventId}",
                eventType3.Id);
        }
        else
        {
            incident = new Incident
            {
                Id = Guid.NewGuid(),
                TypeEnum = IncidentTypeEnum.Type1,
                Time = DateTime.UtcNow,
                Events = new List<Event> { eventType3 }
            };

            _logger.LogInformation(
                "Создан простой инцидент типа 1 на основе события Type3 {EventId}",
                eventType3.Id);
        }

        await _writeRepository.AddAsync(incident);
        await _writeRepository.SaveChangesAsync();
    }
}