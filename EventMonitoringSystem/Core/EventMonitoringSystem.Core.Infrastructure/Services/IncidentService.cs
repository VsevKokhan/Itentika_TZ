using AutoMapper;
using EventMonitoringSystem.Core.Domain.Entities;
using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;
using EventMonitoringSystem.Core.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventMonitoringSystem.Core.Infrastructure.Services;

public class IncidentService : IIncidentService
{
    private readonly ILogger<IncidentService> _logger;
    private readonly IMapper _mapper;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IServiceScopeFactory _scopeFactory;

    public IncidentService(
        ILogger<IncidentService> logger,
        IMapper mapper,
        IIncidentRepository incidentRepository,
        IEventRepository eventRepository,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _mapper = mapper;
        _incidentRepository = incidentRepository;
        _eventRepository = eventRepository;
        _scopeFactory = scopeFactory;
    }

    public async Task<IReadOnlyCollection<Incident>> GetAllIncidentsAsync()
    {
        var incidents = await _incidentRepository.GetAllAsync();
        return incidents;
    }
    
    public async Task ProcessAsync(Event eventDto)
    {
        _logger.LogInformation("Получено событие {EventId} типа {EventTypeEnum}",
            eventDto.Id, eventDto.TypeEnum);

        switch (eventDto.TypeEnum)
        {
            case EventTypeEnum.Type1:
                await ProcessSimpleTemplate(new Event { Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time}) ;
                break;

            case EventTypeEnum.Type2:
                await ProcessCompositeTemplate2(new Event { Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time });
                break;

            case EventTypeEnum.Type3:
                await ProcessCompositeTemplate3(new Event { Id = eventDto.Id, TypeEnum = eventDto.TypeEnum, Time = eventDto.Time} );
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
        };
        await _incidentRepository.AddAsync(incident);

        ev.IncidentId = incident.Id;
        await _eventRepository.AddAsync(ev);

        _logger.LogInformation("Создан простой инцидент типа 1 на основе события {EventId}", ev.Id);
    }

    private async Task ProcessCompositeTemplate2(Event eventType2)
    {
        _logger.LogInformation("Запущена отложенная проверка Type2 для события {EventId}", eventType2.Id);

        _ = Task.Run(async () => await ProcessCompositeType2Delayed(eventType2));
    }

    private async Task ProcessCompositeType2Delayed(Event eventType2)
    {
        await Task.Delay(TimeSpan.FromSeconds(20));

        using var scope = _scopeFactory.CreateScope();
        var eventRepo = scope.ServiceProvider.GetRequiredService<IEventRepository>();
        var incidentRepo = scope.ServiceProvider.GetRequiredService<IIncidentRepository>();

        var allEvents = await eventRepo.GetAllAsync();
        var type1Event = allEvents
            .Where(e => e.TypeEnum == EventTypeEnum.Type1 &&
                        e.Time >= eventType2.Time &&
                        e.Time <= eventType2.Time.AddSeconds(20))
            .OrderBy(e => e.Time)
            .FirstOrDefault();

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            TypeEnum = type1Event != null ? IncidentTypeEnum.Type2 : IncidentTypeEnum.Type1,
            Time = DateTime.UtcNow,
        };

        await incidentRepo.AddAsync(incident);

        eventType2.IncidentId = incident.Id;

        if (type1Event != null)
        {
            type1Event.IncidentId = incident.Id;
            await eventRepo.UpdateAsync(type1Event.Id, type1Event);
        }

        await eventRepo.AddAsync(eventType2);

        _logger.LogInformation(type1Event != null
            ? "Создан составной инцидент Type2 по событиям {A} и {B}"
            : "Создан простой инцидент Type1 на основе события {EventId}",
            type1Event?.Id, eventType2.Id);
    }

    private async Task ProcessCompositeTemplate3(Event eventType3)
    {
        _logger.LogInformation("Запущена отложенная проверка Type3 для события {EventId}", eventType3.Id);

        _ = Task.Run(async () => await ProcessCompositeType3Delayed(eventType3));
    }

    private async Task ProcessCompositeType3Delayed(Event eventType3)
    {
        await Task.Delay(TimeSpan.FromSeconds(60));

        using var scope = _scopeFactory.CreateScope();
        var eventRepo = scope.ServiceProvider.GetRequiredService<IEventRepository>();
        var incidentRepo = scope.ServiceProvider.GetRequiredService<IIncidentRepository>();

        var allIncidents = await incidentRepo.GetAllAsync();
        var incidentType2 = allIncidents
            .Where(i => i.TypeEnum == IncidentTypeEnum.Type2 &&
                        i.Time >= eventType3.Time &&
                        i.Time <= eventType3.Time.AddSeconds(60))
            .OrderBy(i => i.Time)
            .FirstOrDefault();

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            TypeEnum = incidentType2 != null ? IncidentTypeEnum.Type3 : IncidentTypeEnum.Type1,
            Time = DateTime.UtcNow
        };

        await incidentRepo.AddAsync(incident);

        eventType3.IncidentId = incident.Id;

        await eventRepo.AddAsync(eventType3);

        if (incidentType2 != null)
        {
            _logger.LogInformation("Создан составной инцидент Type3 по событию {EventId}", eventType3.Id);
        }
        else
        {
            _logger.LogInformation("Создан простой инцидент Type1 на основе события {EventId}", eventType3.Id);
        }
    }


}