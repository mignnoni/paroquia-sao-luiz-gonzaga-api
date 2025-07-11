using MediatR;
using Ardalis.Result;

namespace BuildingBlocks.Application;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResult> : IRequest<Result<TResult>>
{

}