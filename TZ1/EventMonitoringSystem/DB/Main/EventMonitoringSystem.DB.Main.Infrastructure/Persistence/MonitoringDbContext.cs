using EventMonitoringSystem.DB.Main.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.DB.Main.Infrastructure.Persistence;

public class MonitoringDbContext : DbContext
{
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Event> Events { get; set; }

    public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.TypeEnum).IsRequired();
            entity.Property(i => i.Time).IsRequired();

            entity.HasMany(i => i.Events)
                .WithOne(e => e.Incident)
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TypeEnum).IsRequired();
            entity.Property(e => e.Time).IsRequired();
        });
    }
}