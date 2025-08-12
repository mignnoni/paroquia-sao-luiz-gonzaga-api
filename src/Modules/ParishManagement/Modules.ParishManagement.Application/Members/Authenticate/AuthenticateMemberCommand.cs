using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.Authenticate;

public sealed record AuthenticateMemberCommand(string Email, string Password) : ICommand<AuthenticateMemberResponse>;