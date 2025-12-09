using EventMonitoringSystem.Core.Domain.interfaces;
using EventMonitoringSystem.Core.Infrastructure.Services;
using EventMonitoringSystem.EventGenerator.Infrastructure;

namespace EventMonitoringSystem.EventGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddLogging();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
        builder.Services.AddHostedService<EventGeneratorBackgroundService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}