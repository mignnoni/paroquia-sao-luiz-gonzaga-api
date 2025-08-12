namespace Modules.ParishManagement.Application.Members.Authenticate;

public sealed record AuthenticateMemberResponse(AuthenticatedMember Member, string Token);

public sealed record AuthenticatedMember(string FullName, string Email);