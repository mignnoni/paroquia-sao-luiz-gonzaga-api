using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Application.EventBus;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Domain.Extensions;
using Modules.IdentityProvider.Domain.Users;
using Modules.IdentityProvider.IntegrationEvents;

namespace Modules.IdentityProvider.Application.UpdatePassword;

internal sealed class UpdatePasswordCommandHandler(
    UserManager<User> userManager,
    IEventBus bus) : ICommandHandler<UpdatePasswordCommand>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IEventBus _bus = bus;

    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return Result.Error("As senhas digitadas devem ser iguais");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return Result.Error("Usuário não encontrado");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            return Result.Invalid(result.GetValidationErrors());

        await _bus.PublishAsync(new PasswordResetedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            user.FullName,
            user.Email
        ), cancellationToken);

        return Result.Success();
    }
}
