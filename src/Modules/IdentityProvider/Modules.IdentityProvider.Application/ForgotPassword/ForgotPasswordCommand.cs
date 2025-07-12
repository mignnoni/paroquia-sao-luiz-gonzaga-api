using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand<ForgotPasswordResponse>;
