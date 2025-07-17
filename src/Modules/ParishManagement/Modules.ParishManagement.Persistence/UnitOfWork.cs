using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Persistence;

public sealed class UnitOfWork(ParishManagementDbContext context) : IUnitOfWork
{
    private readonly ParishManagementDbContext _context = context;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) 
        => await _context.SaveChangesAsync(cancellationToken);
}

