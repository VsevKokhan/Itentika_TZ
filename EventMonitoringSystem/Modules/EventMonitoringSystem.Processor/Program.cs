using EventMonitoringSystem.Core.Domain.interfaces;
using EventMonitoringSystem.Core.Domain.Interfaces;
using EventMonitoringSystem.Core.Infrastructure.MapperProfiles;
using EventMonitoringSystem.Core.Infrastructure.Services;
using EventMonitoringSystem.Data.Infrastructure.Persistence;
using EventMonitoringSystem.Data.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.Processor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<MonitoringDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();

        builder.Services.AddScoped<IIncidentService, IncidentService>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MonitoringDbContext>();
            db.Database.EnsureCreated();
        }
        app.MapControllers();

        app.Run();
    }
}