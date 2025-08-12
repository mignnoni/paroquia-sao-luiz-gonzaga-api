using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand<PasswordForgottenResponse>;
