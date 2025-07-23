using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger,
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

        var errors = validationResult.AsErrors();

        if (typeof(TResponse) == typeof(Result))
        {
            _logger.LogError("Erro de validação no command {Request}: {Errors}", typeof(TRequest).Name, errors.Select(e => e.ErrorMessage));
            return (TResponse)(object)Result.Invalid(errors);
        }
        else if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];

            var invalidMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result.Invalid), [typeof(List<ValidationError>)]);

            if (invalidMethod is not null)
            {
                _logger.LogError("Erro de validação no command {Request}: {Errors}", typeof(TRequest).Name, errors.Select(e => e.ErrorMessage));
                return (TResponse)invalidMethod.Invoke(null, [errors]);
            }
        }

        throw new Exception(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
    }
}
