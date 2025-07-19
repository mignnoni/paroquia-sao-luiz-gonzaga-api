using System;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Modules.ParishManagement.Persistence.Repositories;

public class GenericRepository<T>(
    ParishManagementDbContext context) : IRepository<T> where T : class
{
    protected readonly ParishManagementDbContext _context = context;

    public void Add(T group)
    {
        _context.Set<T>().Add(group);
    }

    public void Delete(T group)
    {
        _context.Set<T>().Remove(group);
    }

    public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification, true).AnyAsync(cancellationToken);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TResult>> ListAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
    {
        return SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
    }

}
