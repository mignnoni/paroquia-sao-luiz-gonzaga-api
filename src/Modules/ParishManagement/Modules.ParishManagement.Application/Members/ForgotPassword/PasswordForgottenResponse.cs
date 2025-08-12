namespace Modules.ParishManagement.Application.Members.ForgotPassword;

public sealed record PasswordForgottenResponse(string Email, string Token);
