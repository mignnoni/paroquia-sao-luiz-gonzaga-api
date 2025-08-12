using Modules.ParishManagement.Domain.Members;

namespace ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;

public sealed record UpdateMemberRequest(
    string Name,
    MemberType MemberType);