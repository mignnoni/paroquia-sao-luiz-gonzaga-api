using Modules.ParishManagement.Domain.Members;

namespace ParoquiaSLG.API.Modules.ParishManagement.PendingMembers.Contracts;

public sealed record AddPendingMemberRequest(
    string FullName,
    string Email,
    MemberType MemberType,
    string? Observation = null);
