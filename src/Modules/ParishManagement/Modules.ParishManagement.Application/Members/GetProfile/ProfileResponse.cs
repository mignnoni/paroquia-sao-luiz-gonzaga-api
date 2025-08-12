using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetProfile;

public sealed record ProfileResponse(
    Guid Id,
    string Name,
    string Email,
    MemberType MemberType);
