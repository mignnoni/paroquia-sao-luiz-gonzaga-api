using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Application.EventBus;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Domain.Extensions;
using Modules.IdentityProvider.Domain.Users;
using Modules.IdentityProvider.IntegrationEvents;

namespace Modules.IdentityProvider.Application.ResetPassword;

internal sealed class ResetPasswordCommandHandler(UserManager<User> userManager, IEventBus bus) : ICommandHandler<ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IEventBus _bus = bus;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return Result.Error("As senhas digitadas devem ser iguais");

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.NotFound("Usuário não encontrado");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
            return Result.Invalid(result.GetValidationErrors());

        await _bus.PublishAsync(new PasswordResetedIntegrationEvent(
           Guid.NewGuid(),
           DateTime.UtcNow,
           user.FullName,
           request.Email
        ), cancellationToken);

        return Result.Success();
    }
}
