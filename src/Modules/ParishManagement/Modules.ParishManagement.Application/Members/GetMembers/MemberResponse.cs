namespace Modules.ParishManagement.Application.Members.GetMembers;

public sealed record MemberResponse(Guid Id, string FullName, string Email, string MemberType);
