using Ardalis.Specification;

namespace BuildingBlocks.Domain;

public interface IRepository<T> where T : class
{
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<TResult?> FirstOrDefaultAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken = default);
    Task<List<TResult>> ListAsync<TResult>(Specification<T, TResult> specification, CancellationToken cancellationToken);
    void Add(T entity);
    void Delete(T entity);
}

