using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Application.EventBus;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Application.Common;
using Modules.IdentityProvider.Domain.Users;
using Modules.IdentityProvider.IntegrationEvents;
using System.Text;
using System.Text.Json;

namespace Modules.IdentityProvider.Application.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(UserManager<User> userManager, IEventBus bus) : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IEventBus _bus = bus;

    public async Task<Result<ForgotPasswordResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Result.NotFound("Usuário não encontrado");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (string.IsNullOrEmpty(token))
            return Result.Error("Não foi possível gerar o token de alteração de senha. Por favor, tente novamente");

        string tokenString = JsonSerializer.Serialize(new ResetPasswordToken(token, request.Email));

        byte[] bytes = Encoding.UTF8.GetBytes(tokenString);

        var base64Token = Convert.ToBase64String(bytes);

        await _bus.PublishAsync(new ForgotPasswordIntegrationEvent(
               Guid.NewGuid(),
               DateTime.UtcNow,
               user.FullName,
               request.Email,
               base64Token
           ), cancellationToken);

        return new ForgotPasswordResponse(request.Email, token);
    }
}
