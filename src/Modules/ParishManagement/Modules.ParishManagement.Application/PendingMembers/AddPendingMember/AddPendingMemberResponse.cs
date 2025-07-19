using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.PendingMembers.AddPendingMember;

public sealed record AddPendingMemberResponse(
    Guid Id,
    string FullName,
    string Email,
    string Token,
    MemberType MemberType);
