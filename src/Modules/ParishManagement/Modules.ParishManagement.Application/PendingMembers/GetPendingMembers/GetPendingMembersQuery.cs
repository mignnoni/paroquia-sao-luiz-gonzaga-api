using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMembers;

public sealed record GetPendingMembersQuery(
    int PageIndex,
    int PageSize) : IQuery<List<PendingMemberResponse>>;
