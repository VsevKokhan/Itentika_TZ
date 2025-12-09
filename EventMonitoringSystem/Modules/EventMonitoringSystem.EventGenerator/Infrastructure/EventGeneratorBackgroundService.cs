using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;

namespace EventMonitoringSystem.EventGenerator.Infrastructure;

public class EventGeneratorBackgroundService : BackgroundService
{
    private readonly IEventPublisher _publisher;
    private readonly ILogger<EventGeneratorBackgroundService> _logger;
    private readonly Random _random = new();

    public EventGeneratorBackgroundService(
        IEventPublisher publisher,
        ILogger<EventGeneratorBackgroundService> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Фоновый сервис генерации событий запущен");

        while (!stoppingToken.IsCancellationRequested)
        {
            var eventType = (EventTypeEnum)_random.Next(1, 4);

            _logger.LogInformation("Сгенерировано событие типа {TypeEnum}", eventType);

            try
            {
                await _publisher.PublishAsync(eventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось опубликовать событие {TypeEnum}", eventType);
            }

            // Ждем случайное время от 0 до 2 секунд
            var delay = _random.Next(0, 2000);
            await Task.Delay(delay, stoppingToken);
        }

        _logger.LogInformation("Фоновый сервис генерации событий останавливается");
    }
}
