using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetMemberById;

public sealed record MemberByIdResponse(
    Guid Id,
    MemberType MemberType,
    string Name,
    string Email);
