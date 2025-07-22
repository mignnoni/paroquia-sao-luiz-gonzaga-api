using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var validationResult = await _validators.First().ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next(cancellationToken);

        if (typeof(TResponse) == typeof(Result<TResponse>) || typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Invalid(validationResult.AsErrors());
        }

        throw new Exception(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
    }
}
