using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public sealed class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];

            var isSuccessProperty = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetProperty(nameof(Result.IsSuccess))
                ?.GetValue(response) as bool?;

            if (isSuccessProperty is not null && !isSuccessProperty.Value)
            {
                var errorsProperty = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetProperty(nameof(Result.Errors));

                if (errorsProperty is not null && errorsProperty.GetValue(response) is not null)
                {
                    var errors = errorsProperty.GetValue(response);
                    _logger.LogError("Erro ao executar {Request}: {Errors}", typeof(TRequest).Name, errors);
                }
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            var isSuccessProperty = typeof(Result)
                .GetProperty(nameof(Result.IsSuccess))
                ?.GetValue(response) as bool?;

            if (isSuccessProperty is not null && !isSuccessProperty.Value)
            {
                var errorsProperty = typeof(Result).GetProperty(nameof(Result.Errors));

                if (errorsProperty is not null && errorsProperty.GetValue(response) is not null)
                {
                    var errors = errorsProperty.GetValue(response);
                    _logger.LogError("Erro ao executar {Request}: {Errors}", typeof(TRequest).Name, errors);
                }
            }
        }

        return response;
    }
}

