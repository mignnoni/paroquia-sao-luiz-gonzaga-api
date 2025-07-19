using BuildingBlocks.Application;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.PendingMembers.AddPendingMember;

public sealed record AddPendingMemberCommand(
    string FullName,
    string Email,
    MemberType MemberType) : ICommand<AddPendingMemberResponse>;
