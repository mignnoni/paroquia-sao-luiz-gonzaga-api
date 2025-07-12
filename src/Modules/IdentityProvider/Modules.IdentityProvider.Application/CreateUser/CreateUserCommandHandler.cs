using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Domain.Users.Interfaces;

namespace Modules.IdentityProvider.Application.CreateUser;

public class CreateUserCommandHandler(ICreateUserService _createUserService) : ICommandHandler<CreateUserCommand, UserCreatedResponse>
{
    public async Task<Result<UserCreatedResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _createUserService
            .CreateUser(
                request.FullName,
                request.Email,
                request.Password,
                request.Role
            );

        return result
            .Map(userId => new UserCreatedResponse(
                userId, 
                request.FullName, 
                request.Email));
    }
}
