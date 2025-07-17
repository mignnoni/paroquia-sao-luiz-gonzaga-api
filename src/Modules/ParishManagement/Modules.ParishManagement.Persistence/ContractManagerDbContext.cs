
using BuildingBlocks.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.ParishManagement.Persistence.Constants;

namespace Modules.ParishManagement.Persistence;

public class ParishManagementDbContext(
    DbContextOptions<ParishManagementDbContext> options,
    IPublisher _publisher
    ) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schemas.ParishManagement);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (_publisher is null) return result;

        var entitiesWithEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Count != 0)
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.GetDomainEvents().ToArray();

            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                await _publisher.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }

        }

        return result;
    }
}
