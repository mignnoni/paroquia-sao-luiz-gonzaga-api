using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Domain.Extensions;
using Modules.IdentityProvider.Domain.Users;
using System.Security.Claims;

namespace Modules.IdentityProvider.Application.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager
    ) : ICommandHandler<UpdateUserCommand>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            return Result.Error("Usuário não encontrado");

        if (request.Name == user.FullName && string.IsNullOrWhiteSpace(request.NewRole))
            return Result.Error("Os dados do usuário já estão atualizados");

        if (request.Name.Length <= 3)
            return Result.Error("O nome deve ter mais de 3 caracteres");

        if (!string.IsNullOrWhiteSpace(request.NewRole))
        {
            if (request.UserRequestId == request.UserId)
                return Result.Error("O usuário não pode alterar sua própria role");

            var result = await ChangeUserRole(user, request.OldRole, request.NewRole);

            if (!result.IsSuccess)
                return result;
        }

        if (request.Name != user.FullName)
        {
            user.SetName(request.Name);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result.Error(result.GetValidationErrorsString());

            var claims = await _userManager.GetClaimsAsync(user);

            Claim newClaim = new(ClaimTypes.Name, request.Name);

            if (claims.Any())
            {
                var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (nameClaim is null)
                {
                    await _userManager.AddClaimAsync(user, newClaim);
                }
                else
                {
                    await _userManager.ReplaceClaimAsync(user, nameClaim, newClaim);
                }
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }
        }

        return Result.Success();
    }

    private async Task<Result> ChangeUserRole(User user, string currentRole, string newRole)
    {
        if (currentRole == newRole)
            return Result.Error("A role antiga e a role nova informadas são iguais");

        IList<string> userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Any(x => x == newRole))
            return Result.Error("O usuário já está cadastrado nessa Role");

        if (!userRoles.Any(x => x == currentRole))
            return Result.Error("O usuário não está cadastrado na role atual");

        if (!await _roleManager.RoleExistsAsync(newRole))
            return Result.Error("Nova Role não encontrada");

        var result = await _userManager.RemoveFromRoleAsync(user, currentRole);

        if (!result.Succeeded)
            return Result.Error(result.GetValidationErrorsString());

        result = await _userManager.AddToRoleAsync(user, newRole);

        if (!result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, currentRole);
            return Result.Error(result.GetValidationErrorsString());
        }

        return Result.Success();
    }
}
