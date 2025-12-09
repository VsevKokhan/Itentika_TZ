using System.Net.Http.Json;
using EventMonitoringSystem.Core.Domain.Entities;
using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;
using Microsoft.Extensions.Logging;

namespace EventMonitoringSystem.Core.Infrastructure.Services;

public class EventPublisher : IEventPublisher
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _httpClient = new HttpClient ();
        _logger = logger;
    }

    public async Task PublishAsync(EventTypeEnum typeEnum)
    {
        var evt = new Event {Id = Guid.NewGuid(), TypeEnum = typeEnum, Time = DateTime.UtcNow};

        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7141/api/events", evt);
            _logger.LogInformation($"Sent event {evt.Id}, typeEnum {typeEnum}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error sending event {evt.Id}, typeEnum {typeEnum}, statusCode : {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send event {Id}", evt.Id);
            throw;
        }
    }
}