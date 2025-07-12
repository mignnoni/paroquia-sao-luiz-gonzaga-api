using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Domain.Extensions;
using Modules.IdentityProvider.Domain.Users;

namespace Modules.IdentityProvider.Application.DeleteUser;

public class DeleteUserCommandHandler(UserManager<User> _userManager) 
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user is null)
            return Result.NotFound("Usuário não encontrado");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return Result.Invalid(result.GetValidationErrors());

        return Result.Success();
    }
}
