using BuildingBlocks.Application;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.UpdateMember;

public sealed record UpdateMemberCommand(
    Guid CurrentUserIdentityProviderId,
    Guid Id,
    string Name,
    MemberType MemberType) : ICommand;