using System.Net.Http.Json;
using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;
using EventMonitoringSystem.DB.Main.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EventMonitoringSystem.Core.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(HttpClient httpClient, ILogger<EventPublisher> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task PublishAsync(EventTypeEnum typeEnum)
    {
        var evt = new Event {Id = Guid.NewGuid(), TypeEnum = typeEnum, Time = DateTime.UtcNow};

        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7141/api/Events", evt);
            _logger.LogInformation($"отправлено событие {evt.Id}, typeEnum: {typeEnum}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Ошибка отправки события {evt.Id}, typeEnum: {typeEnum}, statusCode : {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка во время отправки события {Id}", evt.Id);
            throw;
        }
    }
}